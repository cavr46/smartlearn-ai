using MediatR;
using SmartLearn.Application.DTOs;

namespace SmartLearn.Application.Commands;

public class CreateCourseCommand : IRequest<CourseDto>
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
    public Guid InstructorId { get; set; }
}

public class UpdateCourseCommand : IRequest<CourseDto>
{
    public Guid Id { get; set; }
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

public class DeleteCourseCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid InstructorId { get; set; }
}

public class PublishCourseCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid InstructorId { get; set; }
}

public class CreateLessonCommand : IRequest<LessonDto>
{
    public Guid CourseId { get; set; }
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

public class UpdateLessonCommand : IRequest<LessonDto>
{
    public Guid Id { get; set; }
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

public class DeleteLessonCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class TranscribeVideoCommand : IRequest<string>
{
    public Guid LessonId { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
}