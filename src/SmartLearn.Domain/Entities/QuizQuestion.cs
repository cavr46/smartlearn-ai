using SmartLearn.Domain.Common;
using SmartLearn.Domain.Enums;

namespace SmartLearn.Domain.Entities;

public class QuizQuestion : BaseEntity
{
    public string Question { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public QuestionType Type { get; set; }
    public int Points { get; set; }
    public int OrderIndex { get; set; }
    
    // Multiple Choice specific
    public string[] Options { get; set; } = Array.Empty<string>();
    public string[] CorrectAnswers { get; set; } = Array.Empty<string>();
    
    // Foreign keys
    public Guid QuizId { get; set; }
    
    // Navigation properties
    public virtual Quiz Quiz { get; set; } = null!;
    public virtual ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
}