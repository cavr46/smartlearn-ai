using SmartLearn.Domain.Common;
using SmartLearn.Domain.Enums;

namespace SmartLearn.Domain.Entities;

public class QuizAttempt : BaseEntity
{
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal Score { get; set; }
    public bool IsPassed { get; set; }
    public int TimeSpentMinutes { get; set; }
    public AttemptStatus Status { get; set; }
    
    // Foreign keys
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Quiz Quiz { get; set; } = null!;
    public virtual ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
}