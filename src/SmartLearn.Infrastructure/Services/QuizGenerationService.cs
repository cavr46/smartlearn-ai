using Azure.AI.OpenAI;
using SmartLearn.Domain.Entities;
using SmartLearn.Domain.Interfaces;
using System.Text.Json;

namespace SmartLearn.Infrastructure.Services;

public class QuizGenerationService : IQuizGenerationService
{
    private readonly OpenAIClient _openAIClient;
    private readonly string _deploymentName;

    public QuizGenerationService(IConfiguration configuration)
    {
        var endpoint = configuration["AzureOpenAI:Endpoint"] ?? throw new InvalidOperationException("Azure OpenAI endpoint not configured");
        var apiKey = configuration["AzureOpenAI:ApiKey"] ?? throw new InvalidOperationException("Azure OpenAI API key not configured");
        _deploymentName = configuration["AzureOpenAI:DeploymentName"] ?? "gpt-4";
        
        _openAIClient = new OpenAIClient(new Uri(endpoint), new Azure.AzureKeyCredential(apiKey));
    }

    public async Task<IEnumerable<QuizQuestion>> GenerateQuizQuestionsAsync(
        string content, 
        int questionCount = 5, 
        QuestionType questionType = QuestionType.MultipleChoice,
        CancellationToken cancellationToken = default)
    {
        var prompt = BuildPrompt(content, questionCount, questionType);
        
        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = _deploymentName,
            Messages =
            {
                new ChatRequestSystemMessage("You are an expert educator and quiz creator. Generate high-quality quiz questions based on the provided content."),
                new ChatRequestUserMessage(prompt)
            },
            MaxTokens = 2000,
            Temperature = 0.7f
        };

        var response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions, cancellationToken);
        var generatedContent = response.Value.Choices[0].Message.Content;

        return ParseQuizQuestions(generatedContent, questionType);
    }

    public async Task<Quiz> GenerateQuizFromLessonAsync(
        Lesson lesson, 
        QuizType quizType = QuizType.Practice,
        CancellationToken cancellationToken = default)
    {
        var content = $"Lesson Title: {lesson.Title}\n\nLesson Description: {lesson.Description}\n\nLesson Content: {lesson.Content}";
        
        if (!string.IsNullOrEmpty(lesson.VideoTranscript))
        {
            content += $"\n\nVideo Transcript: {lesson.VideoTranscript}";
        }

        var questions = await GenerateQuizQuestionsAsync(content, 5, QuestionType.MultipleChoice, cancellationToken);

        var quiz = new Quiz
        {
            Id = Guid.NewGuid(),
            Title = $"Quiz: {lesson.Title}",
            Description = $"Practice quiz for lesson: {lesson.Title}",
            Type = quizType,
            TimeLimit = 30, // 30 minutes
            MaxAttempts = 3,
            PassingScore = 70.0,
            IsActive = true,
            IsAIGenerated = true,
            AIPrompt = content,
            LessonId = lesson.Id,
            CourseId = lesson.CourseId,
            CreatedById = lesson.Course.InstructorId,
            Questions = questions.ToList()
        };

        return quiz;
    }

    private string BuildPrompt(string content, int questionCount, QuestionType questionType)
    {
        var questionTypeInstructions = questionType switch
        {
            QuestionType.MultipleChoice => "Generate multiple choice questions with 4 options each. Indicate the correct answer.",
            QuestionType.TrueFalse => "Generate true/false questions.",
            QuestionType.FillInTheBlank => "Generate fill-in-the-blank questions with clear blanks marked by underscores.",
            QuestionType.Essay => "Generate essay questions that require detailed explanations.",
            _ => "Generate multiple choice questions with 4 options each."
        };

        return $@"
Based on the following content, generate {questionCount} high-quality quiz questions.

{questionTypeInstructions}

Content:
{content}

Requirements:
- Questions should test understanding, not just memorization
- Provide clear, unambiguous questions
- For multiple choice, include 4 options with only one correct answer
- Include brief explanations for correct answers
- Ensure questions are appropriately challenging but fair
- Format the response as valid JSON

Expected JSON format:
{{
  ""questions"": [
    {{
      ""question"": ""What is...?"",
      ""options"": [""Option A"", ""Option B"", ""Option C"", ""Option D""],
      ""correctAnswers"": [""Option A""],
      ""explanation"": ""Brief explanation of why this is correct"",
      ""points"": 1
    }}
  ]
}}

Please generate the quiz questions now:";
    }

    private IEnumerable<QuizQuestion> ParseQuizQuestions(string jsonResponse, QuestionType questionType)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var quizData = JsonSerializer.Deserialize<QuizGenerationResponse>(jsonResponse, options);
            
            if (quizData?.Questions == null)
            {
                return Enumerable.Empty<QuizQuestion>();
            }

            return quizData.Questions.Select((q, index) => new QuizQuestion
            {
                Id = Guid.NewGuid(),
                Question = q.Question,
                Type = questionType,
                Options = q.Options ?? Array.Empty<string>(),
                CorrectAnswers = q.CorrectAnswers ?? Array.Empty<string>(),
                Explanation = q.Explanation,
                Points = q.Points > 0 ? q.Points : 1,
                OrderIndex = index + 1,
                IsActive = true
            });
        }
        catch (JsonException)
        {
            // Fallback: create a simple question if JSON parsing fails
            return new[]
            {
                new QuizQuestion
                {
                    Id = Guid.NewGuid(),
                    Question = "What was the main topic of this lesson?",
                    Type = QuestionType.MultipleChoice,
                    Options = new[] { "Topic A", "Topic B", "Topic C", "Topic D" },
                    CorrectAnswers = new[] { "Topic A" },
                    Explanation = "This question was generated as a fallback.",
                    Points = 1,
                    OrderIndex = 1,
                    IsActive = true
                }
            };
        }
    }

    private class QuizGenerationResponse
    {
        public QuizQuestionData[] Questions { get; set; } = Array.Empty<QuizQuestionData>();
    }

    private class QuizQuestionData
    {
        public string Question { get; set; } = string.Empty;
        public string[] Options { get; set; } = Array.Empty<string>();
        public string[] CorrectAnswers { get; set; } = Array.Empty<string>();
        public string? Explanation { get; set; }
        public int Points { get; set; } = 1;
    }
}