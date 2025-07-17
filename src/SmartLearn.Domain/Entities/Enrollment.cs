namespace SmartLearn.Domain.Entities;

public class Enrollment
{
    public Guid Id { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    public double Progress { get; set; } = 0.0;
    public DateTime? LastAccessedAt { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? PaymentTransactionId { get; set; }
    public DateTime? CertificateIssuedAt { get; set; }
    public string? CertificateUrl { get; set; }
    
    // Foreign Keys
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
}

public enum EnrollmentStatus
{
    Active,
    Completed,
    Cancelled,
    Refunded
}