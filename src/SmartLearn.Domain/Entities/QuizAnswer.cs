using SmartLearn.Domain.Common;

namespace SmartLearn.Domain.Entities;

public class QuizAnswer : BaseEntity
{
    public string[] SelectedAnswers { get; set; } = Array.Empty<string>();
    public string? TextAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public int PointsEarned { get; set; }
    
    // Foreign keys
    public Guid QuestionId { get; set; }
    public Guid AttemptId { get; set; }
    
    // Navigation properties
    public virtual QuizQuestion Question { get; set; } = null!;
    public virtual QuizAttempt Attempt { get; set; } = null!;
}