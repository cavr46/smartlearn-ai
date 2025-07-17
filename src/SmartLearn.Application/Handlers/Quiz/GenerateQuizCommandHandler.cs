using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartLearn.Application.Commands.Quiz;
using SmartLearn.Application.DTOs.Quiz;
using SmartLearn.Application.Interfaces;
using SmartLearn.Application.Common.Exceptions;
using SmartLearn.Domain.Entities;
using SmartLearn.Domain.Enums;
using System.Text.Json;

namespace SmartLearn.Application.Handlers.Quiz;

public class GenerateQuizCommandHandler : IRequestHandler<GenerateQuizCommand, GeneratedQuizDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IAIService _aiService;
    private readonly ICurrentUserService _currentUserService;

    public GenerateQuizCommandHandler(
        IApplicationDbContext context,
        IAIService aiService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _aiService = aiService;
        _currentUserService = currentUserService;
    }

    public async Task<GeneratedQuizDto> Handle(GenerateQuizCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);

        if (course == null)
        {
            throw new NotFoundException("Course", request.CourseId);
        }

        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            throw new UnauthorizedException("User must be authenticated");
        }

        // Generate quiz using AI
        var aiResponse = await _aiService.GenerateQuizAsync(
            request.Content,
            request.QuestionCount,
            request.Difficulty);

        var quizData = JsonSerializer.Deserialize<AIQuizResponse>(aiResponse);
        
        if (quizData?.Questions == null || !quizData.Questions.Any())
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                ["Content"] = new[] { "Failed to generate quiz from content" }
            });
        }

        // Create quiz entity
        var quiz = new SmartLearn.Domain.Entities.Quiz
        {
            Title = request.Title,
            Description = request.Description,
            Type = QuizType.Practice,
            CourseId = request.CourseId,
            LessonId = request.LessonId,
            CreatedById = userId.Value,
            TimeLimit = 0,
            PassingScore = 70,
            MaxAttempts = 3,
            IsAIGenerated = true,
            ShuffleQuestions = true,
            ShowCorrectAnswers = true
        };

        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync(cancellationToken);

        // Create questions
        var questions = new List<QuizQuestion>();
        for (int i = 0; i < quizData.Questions.Count; i++)
        {
            var aiQuestion = quizData.Questions[i];
            var question = new QuizQuestion
            {
                QuizId = quiz.Id,
                Question = aiQuestion.Question,
                Type = QuestionType.MultipleChoice,
                Options = aiQuestion.Options.ToArray(),
                CorrectAnswers = new[] { aiQuestion.CorrectAnswer },
                Explanation = aiQuestion.Explanation,
                Points = 1,
                OrderIndex = i
            };

            questions.Add(question);
        }

        _context.QuizQuestions.AddRange(questions);
        await _context.SaveChangesAsync(cancellationToken);

        // Return DTO
        return new GeneratedQuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Description = quiz.Description,
            Questions = questions.Select(q => new GeneratedQuestionDto
            {
                Id = q.Id,
                Question = q.Question,
                Options = q.Options.ToList(),
                CorrectAnswer = q.CorrectAnswers.First(),
                Explanation = q.Explanation ?? string.Empty
            }).ToList()
        };
    }

    private class AIQuizResponse
    {
        public List<AIQuestionResponse> Questions { get; set; } = new();
    }

    private class AIQuestionResponse
    {
        public string Question { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public string CorrectAnswer { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }
}