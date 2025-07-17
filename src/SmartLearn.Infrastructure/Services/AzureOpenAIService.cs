using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using SmartLearn.Application.Interfaces;
using System.Text.Json;

namespace SmartLearn.Infrastructure.Services;

public class AzureOpenAIService : IAIService
{
    private readonly OpenAIClient _openAIClient;
    private readonly string _deploymentName;

    public AzureOpenAIService(IConfiguration configuration)
    {
        var endpoint = configuration["AzureOpenAI:Endpoint"];
        var apiKey = configuration["AzureOpenAI:ApiKey"];
        _deploymentName = configuration["AzureOpenAI:DeploymentName"] ?? "gpt-4";

        _openAIClient = new OpenAIClient(new Uri(endpoint!), new AzureKeyCredential(apiKey!));
    }

    public async Task<string> GenerateQuizAsync(string content, int questionCount, string difficulty)
    {
        var prompt = $@"
Create a quiz with {questionCount} multiple choice questions based on the following content.
Difficulty level: {difficulty}
Content: {content}

Format the response as JSON with the following structure:
{{
  ""questions"": [
    {{
      ""question"": ""Question text"",
      ""options"": [""Option A"", ""Option B"", ""Option C"", ""Option D""],
      ""correctAnswer"": ""Option A"",
      ""explanation"": ""Explanation of why this is correct""
    }}
  ]
}}

Make sure questions are relevant to the content and appropriate for the {difficulty} level.
";

        var response = await _openAIClient.GetChatCompletionsAsync(
            new ChatCompletionsOptions(_deploymentName, new ChatRequestMessage[]
            {
                new ChatRequestSystemMessage("You are an expert quiz creator. Create high-quality educational quizzes."),
                new ChatRequestUserMessage(prompt)
            })
            {
                MaxTokens = 2000,
                Temperature = 0.7f
            });

        return response.Value.Choices[0].Message.Content;
    }

    public async Task<string> GenerateQuizFromTranscriptAsync(string transcript, int questionCount, string difficulty)
    {
        var prompt = $@"
Create a quiz with {questionCount} multiple choice questions based on the following video transcript.
Difficulty level: {difficulty}
Transcript: {transcript}

Focus on key concepts, important facts, and main ideas from the transcript.
Format the response as JSON with the following structure:
{{
  ""questions"": [
    {{
      ""question"": ""Question text"",
      ""options"": [""Option A"", ""Option B"", ""Option C"", ""Option D""],
      ""correctAnswer"": ""Option A"",
      ""explanation"": ""Explanation of why this is correct""
    }}
  ]
}}

Make sure questions test comprehension of the material presented in the video.
";

        var response = await _openAIClient.GetChatCompletionsAsync(
            new ChatCompletionsOptions(_deploymentName, new ChatRequestMessage[]
            {
                new ChatRequestSystemMessage("You are an expert quiz creator specializing in video content assessment."),
                new ChatRequestUserMessage(prompt)
            })
            {
                MaxTokens = 2000,
                Temperature = 0.7f
            });

        return response.Value.Choices[0].Message.Content;
    }

    public async Task<string> GenerateSummaryAsync(string content)
    {
        var prompt = $@"
Create a comprehensive summary of the following content:
{content}

The summary should:
- Highlight key concepts and main ideas
- Be concise but comprehensive
- Include important details
- Be suitable for study purposes
";

        var response = await _openAIClient.GetChatCompletionsAsync(
            new ChatCompletionsOptions(_deploymentName, new ChatRequestMessage[]
            {
                new ChatRequestSystemMessage("You are an expert at creating educational summaries."),
                new ChatRequestUserMessage(prompt)
            })
            {
                MaxTokens = 1000,
                Temperature = 0.5f
            });

        return response.Value.Choices[0].Message.Content;
    }

    public async Task<string> GenerateExplanationAsync(string question, string answer)
    {
        var prompt = $@"
Question: {question}
Answer: {answer}

Provide a detailed explanation of why this answer is correct. Include:
- Clear reasoning
- Relevant context
- Additional information that helps understanding
";

        var response = await _openAIClient.GetChatCompletionsAsync(
            new ChatCompletionsOptions(_deploymentName, new ChatRequestMessage[]
            {
                new ChatRequestSystemMessage("You are an expert educator providing detailed explanations."),
                new ChatRequestUserMessage(prompt)
            })
            {
                MaxTokens = 500,
                Temperature = 0.3f
            });

        return response.Value.Choices[0].Message.Content;
    }
}