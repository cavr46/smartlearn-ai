using SmartLearn.Domain.Common;
using SmartLearn.Domain.Enums;

namespace SmartLearn.Domain.Entities;

public class Enrollment : BaseEntity
{
    public DateTime EnrolledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal Progress { get; set; }
    public EnrollmentStatus Status { get; set; }
    public DateTime? LastAccessedAt { get; set; }
    
    // Foreign keys
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
    public virtual Certificate? Certificate { get; set; }
}