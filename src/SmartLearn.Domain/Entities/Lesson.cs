using SmartLearn.Domain.Common;
using SmartLearn.Domain.Enums;

namespace SmartLearn.Domain.Entities;

public class Lesson : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public LessonType Type { get; set; }
    public string? VideoUrl { get; set; }
    public string? TranscriptUrl { get; set; }
    public string? Content { get; set; }
    public int DurationInMinutes { get; set; }
    public int OrderIndex { get; set; }
    public bool IsFree { get; set; }
    public string[] ResourceUrls { get; set; } = Array.Empty<string>();
    
    // Foreign keys
    public Guid ModuleId { get; set; }
    
    // Navigation properties
    public virtual Module Module { get; set; } = null!;
    public virtual ICollection<LessonProgress> Progress { get; set; } = new List<LessonProgress>();
}