using SmartLearn.Domain.Common;

namespace SmartLearn.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Foreign keys
    public Guid? ParentCategoryId { get; set; }
    
    // Navigation properties
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}