using SmartLearn.Domain.Common;
using SmartLearn.Domain.Enums;

namespace SmartLearn.Domain.Entities;

public class Quiz : BaseEntity, IAggregateRoot
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public QuizType Type { get; set; }
    public int TimeLimit { get; set; } // in minutes
    public int PassingScore { get; set; } // percentage
    public int MaxAttempts { get; set; }
    public bool ShuffleQuestions { get; set; }
    public bool ShowCorrectAnswers { get; set; }
    public bool IsAIGenerated { get; set; }
    
    // Foreign keys
    public Guid CourseId { get; set; }
    public Guid? LessonId { get; set; }
    public Guid CreatedById { get; set; }
    
    // Navigation properties
    public virtual Course Course { get; set; } = null!;
    public virtual Lesson? Lesson { get; set; }
    public virtual User CreatedBy { get; set; } = null!;
    public virtual ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    public virtual ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
}