using MediatR;
using SmartLearn.Application.DTOs.Quiz;

namespace SmartLearn.Application.Commands.Quiz;

public class GenerateQuizCommand : IRequest<GeneratedQuizDto>
{
    public string Content { get; set; } = string.Empty;
    public int QuestionCount { get; set; } = 5;
    public string Difficulty { get; set; } = "Medium";
    public Guid CourseId { get; set; }
    public Guid? LessonId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}