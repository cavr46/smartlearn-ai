using SmartLearn.Domain.Entities;

namespace SmartLearn.Domain.Interfaces;

public interface IQuizGenerationService
{
    Task<IEnumerable<QuizQuestion>> GenerateQuizQuestionsAsync(
        string content, 
        int questionCount = 5, 
        QuestionType questionType = QuestionType.MultipleChoice,
        CancellationToken cancellationToken = default);
        
    Task<Quiz> GenerateQuizFromLessonAsync(
        Lesson lesson, 
        QuizType quizType = QuizType.Practice,
        CancellationToken cancellationToken = default);
}

public interface ITranscriptionService
{
    Task<string> TranscribeAudioAsync(string audioUrl, CancellationToken cancellationToken = default);
    Task<string> TranscribeVideoAsync(string videoUrl, CancellationToken cancellationToken = default);
    Task<string> TranscribeFromBlobAsync(string blobUrl, CancellationToken cancellationToken = default);
}

public interface IContentModerationService
{
    Task<bool> IsContentAppropriateAsync(string content, CancellationToken cancellationToken = default);
    Task<ContentModerationResult> ModerateContentAsync(string content, CancellationToken cancellationToken = default);
}

public interface IPersonalizationService
{
    Task<IEnumerable<Course>> GetRecommendedCoursesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<string> GeneratePersonalizedLearningPathAsync(Guid userId, CancellationToken cancellationToken = default);
}

public class ContentModerationResult
{
    public bool IsAppropriate { get; set; }
    public double ConfidenceScore { get; set; }
    public string[] FlaggedCategories { get; set; } = Array.Empty<string>();
    public string? Reason { get; set; }
}