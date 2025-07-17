using SmartLearn.Domain.Common;

namespace SmartLearn.Domain.Entities;

public class LessonProgress : BaseEntity
{
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int WatchedSeconds { get; set; }
    public int TotalSeconds { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime LastAccessedAt { get; set; }
    
    // Foreign keys
    public Guid UserId { get; set; }
    public Guid LessonId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Lesson Lesson { get; set; } = null!;
    
    public decimal ProgressPercentage => TotalSeconds > 0 ? (decimal)WatchedSeconds / TotalSeconds * 100 : 0;
}