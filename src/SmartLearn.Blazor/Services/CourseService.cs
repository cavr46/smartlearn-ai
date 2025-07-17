using SmartLearn.Application.DTOs;
using System.Net.Http.Json;

namespace SmartLearn.Blazor.Services;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetCoursesAsync();
    Task<CourseDto?> GetCourseAsync(Guid id);
    Task<CourseDto> CreateCourseAsync(CreateCourseDto course);
    Task<CourseDto> UpdateCourseAsync(Guid id, UpdateCourseDto course);
    Task<bool> DeleteCourseAsync(Guid id);
    Task<IEnumerable<CourseDto>> GetFeaturedCoursesAsync();
    Task<IEnumerable<CourseDto>> GetPopularCoursesAsync();
    Task<IEnumerable<CourseDto>> SearchCoursesAsync(string query);
    Task<IEnumerable<CourseDto>> GetMyCoursesAsync();
    Task<IEnumerable<CourseDto>> GetEnrolledCoursesAsync();
    Task<IEnumerable<LessonDto>> GetLessonsAsync(Guid courseId);
    Task<LessonDto> CreateLessonAsync(Guid courseId, CreateLessonDto lesson);
    Task<string> TranscribeLessonAsync(Guid lessonId, string videoUrl);
}

public class CourseService : ICourseService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public CourseService(HttpClient httpClient, IAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
    {
        var response = await _httpClient.GetAsync("api/courses");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<CourseDto>>() ?? Enumerable.Empty<CourseDto>();
    }

    public async Task<CourseDto?> GetCourseAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/courses/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CourseDto>();
        }
        return null;
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto course)
    {
        await EnsureAuthenticatedAsync();
        var response = await _httpClient.PostAsJsonAsync("api/courses", course);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CourseDto>() ?? throw new InvalidOperationException();
    }

    public async Task<CourseDto> UpdateCourseAsync(Guid id, UpdateCourseDto course)
    {
        await EnsureAuthenticatedAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/courses/{id}", course);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CourseDto>() ?? throw new InvalidOperationException();
    }

    public async Task<bool> DeleteCourseAsync(Guid id)
    {
        await EnsureAuthenticatedAsync();
        var response = await _httpClient.DeleteAsync($"api/courses/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<CourseDto>> GetFeaturedCoursesAsync()
    {
        var response = await _httpClient.GetAsync("api/courses/featured");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<CourseDto>>() ?? Enumerable.Empty<CourseDto>();
    }

    public async Task<IEnumerable<CourseDto>> GetPopularCoursesAsync()
    {
        var response = await _httpClient.GetAsync("api/courses/popular");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<CourseDto>>() ?? Enumerable.Empty<CourseDto>();
    }

    public async Task<IEnumerable<CourseDto>> SearchCoursesAsync(string query)
    {
        var response = await _httpClient.GetAsync($"api/courses/search?query={Uri.EscapeDataString(query)}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<CourseDto>>() ?? Enumerable.Empty<CourseDto>();
    }

    public async Task<IEnumerable<CourseDto>> GetMyCoursesAsync()
    {
        await EnsureAuthenticatedAsync();
        var response = await _httpClient.GetAsync("api/courses/my-courses");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<CourseDto>>() ?? Enumerable.Empty<CourseDto>();
    }

    public async Task<IEnumerable<CourseDto>> GetEnrolledCoursesAsync()
    {
        await EnsureAuthenticatedAsync();
        var response = await _httpClient.GetAsync("api/courses/enrolled");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<CourseDto>>() ?? Enumerable.Empty<CourseDto>();
    }

    public async Task<IEnumerable<LessonDto>> GetLessonsAsync(Guid courseId)
    {
        var response = await _httpClient.GetAsync($"api/courses/{courseId}/lessons");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<LessonDto>>() ?? Enumerable.Empty<LessonDto>();
    }

    public async Task<LessonDto> CreateLessonAsync(Guid courseId, CreateLessonDto lesson)
    {
        await EnsureAuthenticatedAsync();
        var response = await _httpClient.PostAsJsonAsync($"api/courses/{courseId}/lessons", lesson);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LessonDto>() ?? throw new InvalidOperationException();
    }

    public async Task<string> TranscribeLessonAsync(Guid lessonId, string videoUrl)
    {
        await EnsureAuthenticatedAsync();
        var request = new { VideoUrl = videoUrl };
        var response = await _httpClient.PostAsJsonAsync($"api/courses/lessons/{lessonId}/transcribe", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TranscriptionResult>();
        return result?.Transcript ?? "";
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

    private class TranscriptionResult
    {
        public string Transcript { get; set; } = string.Empty;
    }
}