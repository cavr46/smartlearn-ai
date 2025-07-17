namespace SmartLearn.Domain.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public CourseLevel Level { get; set; } = CourseLevel.Beginner;
    public string Category { get; set; } = string.Empty;
    public string[] Tags { get; set; } = Array.Empty<string>();
    public CourseStatus Status { get; set; } = CourseStatus.Draft;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public int EstimatedDurationMinutes { get; set; }
    public string? LearningObjectives { get; set; }
    public string? Prerequisites { get; set; }
    public string Language { get; set; } = "en";
    public bool HasCertificate { get; set; } = true;
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }
    public int EnrollmentCount { get; set; }
    
    // Foreign Keys
    public Guid InstructorId { get; set; }
    
    // Navigation properties
    public virtual User Instructor { get; set; } = null!;
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public virtual ICollection<CourseRating> Ratings { get; set; } = new List<CourseRating>();
}

public enum CourseLevel
{
    Beginner,
    Intermediate,
    Advanced,
    Expert
}

public enum CourseStatus
{
    Draft,
    Published,
    Archived
}