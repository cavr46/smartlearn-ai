using SmartLearn.API.Controllers;
using System.Net.Http.Json;

namespace SmartLearn.Blazor.Services;

public interface IQuizService
{
    Task<QuizDto> GenerateQuizAsync(Guid lessonId, string quizType = "Practice", int questionCount = 5);
    Task<IEnumerable<QuizQuestionDto>> GenerateQuestionsAsync(string content, int questionCount = 5, string questionType = "MultipleChoice");
}

public class QuizService : IQuizService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public QuizService(HttpClient httpClient, IAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<QuizDto> GenerateQuizAsync(Guid lessonId, string quizType = "Practice", int questionCount = 5)
    {
        await EnsureAuthenticatedAsync();
        
        var request = new GenerateQuizRequest
        {
            LessonId = lessonId,
            QuizType = quizType,
            QuestionCount = questionCount
        };

        var response = await _httpClient.PostAsJsonAsync("api/quizzes/generate", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<QuizDto>() ?? throw new InvalidOperationException();
    }

    public async Task<IEnumerable<QuizQuestionDto>> GenerateQuestionsAsync(string content, int questionCount = 5, string questionType = "MultipleChoice")
    {
        await EnsureAuthenticatedAsync();
        
        var request = new GenerateQuestionsRequest
        {
            Content = content,
            QuestionCount = questionCount,
            QuestionType = questionType
        };

        var response = await _httpClient.PostAsJsonAsync("api/quizzes/generate-from-content", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<QuizQuestionDto>>() ?? Enumerable.Empty<QuizQuestionDto>();
    }

    private async Task EnsureAuthenticatedAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }
        
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}