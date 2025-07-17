using SmartLearn.Domain.Common;

namespace SmartLearn.Domain.Entities;

public class Module : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public int EstimatedMinutes { get; set; }
    
    // Foreign keys
    public Guid CourseId { get; set; }
    
    // Navigation properties
    public virtual Course Course { get; set; } = null!;
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}