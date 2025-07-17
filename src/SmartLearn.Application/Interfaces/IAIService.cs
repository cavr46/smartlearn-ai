namespace SmartLearn.Application.Interfaces;

public interface IAIService
{
    Task<string> GenerateQuizAsync(string content, int questionCount, string difficulty);
    Task<string> GenerateQuizFromTranscriptAsync(string transcript, int questionCount, string difficulty);
    Task<string> GenerateSummaryAsync(string content);
    Task<string> GenerateExplanationAsync(string question, string answer);
}