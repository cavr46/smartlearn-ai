using MediatR;
using SmartLearn.API.Controllers;
using SmartLearn.Application.DTOs;
using SmartLearn.Domain.Entities;

namespace SmartLearn.Application.Commands;

public class GenerateQuizCommand : IRequest<QuizDto>
{
    public Guid LessonId { get; set; }
    public string QuizType { get; set; } = "Practice";
    public int QuestionCount { get; set; } = 5;
    public Guid CreatedById { get; set; }
}

public class GenerateQuestionsFromContentCommand : IRequest<IEnumerable<QuizQuestionDto>>
{
    public string Content { get; set; } = string.Empty;
    public int QuestionCount { get; set; } = 5;
    public string QuestionType { get; set; } = "MultipleChoice";
}