using SmartLearn.Domain.Common;
using SmartLearn.Domain.Enums;
using SmartLearn.Domain.ValueObjects;

namespace SmartLearn.Domain.Entities;

public class Course : BaseEntity, IAggregateRoot
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? PreviewVideoUrl { get; set; }
    public Money Price { get; set; } = new Money(0, "USD");
    public CourseLevel Level { get; set; }
    public CourseStatus Status { get; set; }
    public string Language { get; set; } = "en";
    public int DurationInHours { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string[] LearningObjectives { get; set; } = Array.Empty<string>();
    public string[] Requirements { get; set; } = Array.Empty<string>();
    public string[] TargetAudience { get; set; } = Array.Empty<string>();
    
    // SEO
    public string Slug { get; set; } = string.Empty;
    public string? MetaDescription { get; set; }
    
    // Analytics
    public int EnrollmentCount { get; set; }
    public decimal AverageRating { get; set; }
    public int ReviewCount { get; set; }
    
    // Foreign keys
    public Guid InstructorId { get; set; }
    public Guid CategoryId { get; set; }
    
    // Navigation properties
    public virtual User Instructor { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}