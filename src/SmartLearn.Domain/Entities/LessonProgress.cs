namespace SmartLearn.Domain.Entities;

public class LessonProgress
{
    public Guid Id { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public double Progress { get; set; } = 0.0;
    public int TimeSpentMinutes { get; set; } = 0;
    public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
    public string? Notes { get; set; }
    
    // Foreign Keys
    public Guid UserId { get; set; }
    public Guid LessonId { get; set; }
    public Guid EnrollmentId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Lesson Lesson { get; set; } = null!;
    public virtual Enrollment Enrollment { get; set; } = null!;
}