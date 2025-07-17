using SmartLearn.Domain.Common;

namespace SmartLearn.Domain.Entities;

public class Certificate : BaseEntity
{
    public string CertificateNumber { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public string? VerificationUrl { get; set; }
    public string? PdfUrl { get; set; }
    public decimal CompletionPercentage { get; set; }
    public decimal FinalScore { get; set; }
    
    // Foreign keys
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    public Guid EnrollmentId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
    public virtual Enrollment Enrollment { get; set; } = null!;
}