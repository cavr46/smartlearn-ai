using SmartLearn.Domain.Common;
using SmartLearn.Domain.Enums;
using SmartLearn.Domain.ValueObjects;

namespace SmartLearn.Domain.Entities;

public class Payment : BaseEntity, IAggregateRoot
{
    public Money Amount { get; set; } = null!;
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public string? TransactionId { get; set; }
    public string? StripePaymentIntentId { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? FailureReason { get; set; }
    
    // Foreign keys
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}