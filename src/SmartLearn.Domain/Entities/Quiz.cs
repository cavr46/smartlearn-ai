namespace SmartLearn.Domain.Entities;

public class Quiz
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public QuizType Type { get; set; } = QuizType.Practice;
    public int TimeLimit { get; set; } = 0; // 0 means no time limit
    public int MaxAttempts { get; set; } = 3;
    public double PassingScore { get; set; } = 70.0;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsAIGenerated { get; set; } = false;
    public string? AIPrompt { get; set; }
    public int OrderIndex { get; set; }
    
    // Foreign Keys
    public Guid? CourseId { get; set; }
    public Guid? LessonId { get; set; }
    public Guid CreatedById { get; set; }
    
    // Navigation properties
    public virtual Course? Course { get; set; }
    public virtual Lesson? Lesson { get; set; }
    public virtual User CreatedBy { get; set; } = null!;
    public virtual ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    public virtual ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
}

public class QuizQuestion
{
    public Guid Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
    public string[] Options { get; set; } = Array.Empty<string>();
    public string[] CorrectAnswers { get; set; } = Array.Empty<string>();
    public string? Explanation { get; set; }
    public int Points { get; set; } = 1;
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Foreign Keys
    public Guid QuizId { get; set; }
    
    // Navigation properties
    public virtual Quiz Quiz { get; set; } = null!;
    public virtual ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
}

public class QuizAttempt
{
    public Guid Id { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public double Score { get; set; } = 0.0;
    public bool IsPassed { get; set; } = false;
    public int TimeSpentMinutes { get; set; } = 0;
    public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;
    
    // Foreign Keys
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Quiz Quiz { get; set; } = null!;
    public virtual ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
}

public class QuizAnswer
{
    public Guid Id { get; set; }
    public string[] SelectedAnswers { get; set; } = Array.Empty<string>();
    public string? TextAnswer { get; set; }
    public bool IsCorrect { get; set; } = false;
    public int PointsEarned { get; set; } = 0;
    public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
    
    // Foreign Keys
    public Guid QuestionId { get; set; }
    public Guid AttemptId { get; set; }
    
    // Navigation properties
    public virtual QuizQuestion Question { get; set; } = null!;
    public virtual QuizAttempt Attempt { get; set; } = null!;
}

public enum QuizType
{
    Practice,
    Assessment,
    Final
}

public enum QuestionType
{
    MultipleChoice,
    TrueFalse,
    FillInTheBlank,
    Essay,
    Matching
}

public enum AttemptStatus
{
    InProgress,
    Completed,
    Abandoned,
    TimedOut
}