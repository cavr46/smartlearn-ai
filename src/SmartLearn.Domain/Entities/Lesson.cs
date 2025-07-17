namespace SmartLearn.Domain.Entities;

public class Lesson
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public string? VideoTranscript { get; set; }
    public string? AudioUrl { get; set; }
    public LessonType Type { get; set; } = LessonType.Video;
    public int OrderIndex { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsPreview { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Resources { get; set; }
    public string? Notes { get; set; }
    
    // Foreign Keys
    public Guid CourseId { get; set; }
    
    // Navigation properties
    public virtual Course Course { get; set; } = null!;
    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}

public enum LessonType
{
    Video,
    Audio,
    Text,
    Interactive,
    Quiz,
    Assignment
}