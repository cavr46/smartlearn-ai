using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLearn.Application.Commands;
using SmartLearn.Application.DTOs;
using SmartLearn.Application.Queries;
using System.Security.Claims;

namespace SmartLearn.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuizzesController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuizzesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("generate")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<QuizDto>> GenerateQuiz([FromBody] GenerateQuizRequest request)
    {
        var userId = GetCurrentUserId();
        var command = new GenerateQuizCommand
        {
            LessonId = request.LessonId,
            QuizType = request.QuizType,
            QuestionCount = request.QuestionCount,
            CreatedById = userId
        };

        var quiz = await _mediator.Send(command);
        return Ok(quiz);
    }

    [HttpPost("generate-from-content")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<QuizQuestionDto>>> GenerateQuestions([FromBody] GenerateQuestionsRequest request)
    {
        var command = new GenerateQuestionsFromContentCommand
        {
            Content = request.Content,
            QuestionCount = request.QuestionCount,
            QuestionType = request.QuestionType
        };

        var questions = await _mediator.Send(command);
        return Ok(questions);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException());
    }
}

public class GenerateQuizRequest
{
    public Guid LessonId { get; set; }
    public string QuizType { get; set; } = "Practice";
    public int QuestionCount { get; set; } = 5;
}

public class GenerateQuestionsRequest
{
    public string Content { get; set; } = string.Empty;
    public int QuestionCount { get; set; } = 5;
    public string QuestionType { get; set; } = "MultipleChoice";
}

public class QuizDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int TimeLimit { get; set; }
    public int MaxAttempts { get; set; }
    public double PassingScore { get; set; }
    public bool IsAIGenerated { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<QuizQuestionDto> Questions { get; set; } = new();
}

public class QuizQuestionDto
{
    public Guid Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string[] Options { get; set; } = Array.Empty<string>();
    public string[] CorrectAnswers { get; set; } = Array.Empty<string>();
    public string? Explanation { get; set; }
    public int Points { get; set; }
    public int OrderIndex { get; set; }
}