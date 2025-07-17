using MediatR;
using SmartLearn.Application.DTOs;

namespace SmartLearn.Application.Queries;

public class GetCoursesQuery : IRequest<IEnumerable<CourseDto>>
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

public class GetCourseByIdQuery : IRequest<CourseDto?>
{
    public Guid Id { get; set; }
}

public class GetCoursesByInstructorQuery : IRequest<IEnumerable<CourseDto>>
{
    public Guid InstructorId { get; set; }
}

public class GetEnrolledCoursesQuery : IRequest<IEnumerable<CourseDto>>
{
    public Guid UserId { get; set; }
}

public class GetLessonByIdQuery : IRequest<LessonDto?>
{
    public Guid Id { get; set; }
}

public class GetLessonsByCourseQuery : IRequest<IEnumerable<LessonDto>>
{
    public Guid CourseId { get; set; }
}

public class GetPopularCoursesQuery : IRequest<IEnumerable<CourseDto>>
{
    public int Count { get; set; } = 10;
}

public class GetFeaturedCoursesQuery : IRequest<IEnumerable<CourseDto>>
{
    public int Count { get; set; } = 6;
}

public class GetCategoriesQuery : IRequest<IEnumerable<string>>
{
}

public class SearchCoursesQuery : IRequest<IEnumerable<CourseDto>>
{
    public string Query { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}