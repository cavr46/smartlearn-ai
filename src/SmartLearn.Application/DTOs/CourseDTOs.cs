namespace SmartLearn.Application.DTOs;

public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public string Level { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int EstimatedDurationMinutes { get; set; }
    public string? LearningObjectives { get; set; }
    public string? Prerequisites { get; set; }
    public string Language { get; set; } = "en";
    public bool HasCertificate { get; set; }
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }
    public int EnrollmentCount { get; set; }
    public UserDto Instructor { get; set; } = null!;
    public List<LessonDto> Lessons { get; set; } = new();
}

public class CreateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public string Level { get; set; } = "Beginner";
    public string Category { get; set; } = string.Empty;
    public string[] Tags { get; set; } = Array.Empty<string>();
    public int EstimatedDurationMinutes { get; set; }
    public string? LearningObjectives { get; set; }
    public string? Prerequisites { get; set; }
    public string Language { get; set; } = "en";
    public bool HasCertificate { get; set; } = true;
}

public class UpdateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public string Level { get; set; } = "Beginner";
    public string Category { get; set; } = string.Empty;
    public string[] Tags { get; set; } = Array.Empty<string>();
    public int EstimatedDurationMinutes { get; set; }
    public string? LearningObjectives { get; set; }
    public string? Prerequisites { get; set; }
    public string Language { get; set; } = "en";
    public bool HasCertificate { get; set; } = true;
}

public class LessonDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public string? VideoTranscript { get; set; }
    public string? AudioUrl { get; set; }
    public string Type { get; set; } = "Video";
    public int OrderIndex { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsPreview { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Resources { get; set; }
    public string? Notes { get; set; }
}

public class CreateLessonDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public string? AudioUrl { get; set; }
    public string Type { get; set; } = "Video";
    public int OrderIndex { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsPreview { get; set; } = false;
    public string? Resources { get; set; }
    public string? Notes { get; set; }
}

public class CourseSearchDto
{
    public string? Query { get; set; }
    public string? Category { get; set; }
    public string? Level { get; set; }
    public string? Language { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MinPrice { get; set; }
    public bool? HasCertificate { get; set; }
    public string? SortBy { get; set; } = "created";
    public string? SortOrder { get; set; } = "desc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}