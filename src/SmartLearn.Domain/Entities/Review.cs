using SmartLearn.Domain.Common;

namespace SmartLearn.Domain.Entities;

public class Review : BaseEntity
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsVerifiedPurchase { get; set; }
    public int HelpfulCount { get; set; }
    
    // Foreign keys
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}