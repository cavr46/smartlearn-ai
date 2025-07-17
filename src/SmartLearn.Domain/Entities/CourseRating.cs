namespace SmartLearn.Domain.Entities;

public class CourseRating
{
    public Guid Id { get; set; }
    public int Rating { get; set; } // 1-5 stars
    public string? Review { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; } = false;
    public int HelpfulVotes { get; set; } = 0;
    
    // Foreign Keys
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}