using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLearn.Application.Commands;
using SmartLearn.Application.DTOs;
using SmartLearn.Application.Queries;
using System.Security.Claims;

namespace SmartLearn.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses([FromQuery] CourseSearchDto searchDto)
    {
        var query = new GetCoursesQuery
        {
            Query = searchDto.Query,
            Category = searchDto.Category,
            Level = searchDto.Level,
            Language = searchDto.Language,
            MaxPrice = searchDto.MaxPrice,
            MinPrice = searchDto.MinPrice,
            HasCertificate = searchDto.HasCertificate,
            SortBy = searchDto.SortBy,
            SortOrder = searchDto.SortOrder,
            Page = searchDto.Page,
            PageSize = searchDto.PageSize
        };

        var courses = await _mediator.Send(query);
        return Ok(courses);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<CourseDto>> GetCourse(Guid id)
    {
        var query = new GetCourseByIdQuery { Id = id };
        var course = await _mediator.Send(query);
        
        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
    {
        var userId = GetCurrentUserId();
        var command = new CreateCourseCommand
        {
            Title = createCourseDto.Title,
            Description = createCourseDto.Description,
            ShortDescription = createCourseDto.ShortDescription,
            ThumbnailUrl = createCourseDto.ThumbnailUrl,
            Price = createCourseDto.Price,
            Currency = createCourseDto.Currency,
            Level = createCourseDto.Level,
            Category = createCourseDto.Category,
            Tags = createCourseDto.Tags,
            EstimatedDurationMinutes = createCourseDto.EstimatedDurationMinutes,
            LearningObjectives = createCourseDto.LearningObjectives,
            Prerequisites = createCourseDto.Prerequisites,
            Language = createCourseDto.Language,
            HasCertificate = createCourseDto.HasCertificate,
            InstructorId = userId
        };

        var course = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(Guid id, [FromBody] UpdateCourseDto updateCourseDto)
    {
        var command = new UpdateCourseCommand
        {
            Id = id,
            Title = updateCourseDto.Title,
            Description = updateCourseDto.Description,
            ShortDescription = updateCourseDto.ShortDescription,
            ThumbnailUrl = updateCourseDto.ThumbnailUrl,
            Price = updateCourseDto.Price,
            Currency = updateCourseDto.Currency,
            Level = updateCourseDto.Level,
            Category = updateCourseDto.Category,
            Tags = updateCourseDto.Tags,
            EstimatedDurationMinutes = updateCourseDto.EstimatedDurationMinutes,
            LearningObjectives = updateCourseDto.LearningObjectives,
            Prerequisites = updateCourseDto.Prerequisites,
            Language = updateCourseDto.Language,
            HasCertificate = updateCourseDto.HasCertificate
        };

        var course = await _mediator.Send(command);
        return Ok(course);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult> DeleteCourse(Guid id)
    {
        var userId = GetCurrentUserId();
        var command = new DeleteCourseCommand { Id = id, InstructorId = userId };
        var result = await _mediator.Send(command);
        
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("{id}/publish")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult> PublishCourse(Guid id)
    {
        var userId = GetCurrentUserId();
        var command = new PublishCourseCommand { Id = id, InstructorId = userId };
        var result = await _mediator.Send(command);
        
        if (!result)
        {
            return NotFound();
        }

        return Ok(new { message = "Course published successfully" });
    }

    [HttpGet("{id}/lessons")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessons(Guid id)
    {
        var query = new GetLessonsByCourseQuery { CourseId = id };
        var lessons = await _mediator.Send(query);
        return Ok(lessons);
    }

    [HttpPost("{id}/lessons")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<LessonDto>> CreateLesson(Guid id, [FromBody] CreateLessonDto createLessonDto)
    {
        var command = new CreateLessonCommand
        {
            CourseId = id,
            Title = createLessonDto.Title,
            Description = createLessonDto.Description,
            Content = createLessonDto.Content,
            VideoUrl = createLessonDto.VideoUrl,
            AudioUrl = createLessonDto.AudioUrl,
            Type = createLessonDto.Type,
            OrderIndex = createLessonDto.OrderIndex,
            DurationMinutes = createLessonDto.DurationMinutes,
            IsPreview = createLessonDto.IsPreview,
            Resources = createLessonDto.Resources,
            Notes = createLessonDto.Notes
        };

        var lesson = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
    }

    [HttpGet("lessons/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<LessonDto>> GetLesson(Guid id)
    {
        var query = new GetLessonByIdQuery { Id = id };
        var lesson = await _mediator.Send(query);
        
        if (lesson == null)
        {
            return NotFound();
        }

        return Ok(lesson);
    }

    [HttpPut("lessons/{id}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<LessonDto>> UpdateLesson(Guid id, [FromBody] CreateLessonDto updateLessonDto)
    {
        var command = new UpdateLessonCommand
        {
            Id = id,
            Title = updateLessonDto.Title,
            Description = updateLessonDto.Description,
            Content = updateLessonDto.Content,
            VideoUrl = updateLessonDto.VideoUrl,
            AudioUrl = updateLessonDto.AudioUrl,
            Type = updateLessonDto.Type,
            OrderIndex = updateLessonDto.OrderIndex,
            DurationMinutes = updateLessonDto.DurationMinutes,
            IsPreview = updateLessonDto.IsPreview,
            Resources = updateLessonDto.Resources,
            Notes = updateLessonDto.Notes
        };

        var lesson = await _mediator.Send(command);
        return Ok(lesson);
    }

    [HttpDelete("lessons/{id}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult> DeleteLesson(Guid id)
    {
        var command = new DeleteLessonCommand { Id = id };
        var result = await _mediator.Send(command);
        
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("lessons/{id}/transcribe")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<string>> TranscribeLesson(Guid id, [FromBody] TranscribeVideoRequest request)
    {
        var command = new TranscribeVideoCommand
        {
            LessonId = id,
            VideoUrl = request.VideoUrl
        };

        var transcript = await _mediator.Send(command);
        return Ok(new { transcript });
    }

    [HttpGet("featured")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetFeaturedCourses()
    {
        var query = new GetFeaturedCoursesQuery { Count = 6 };
        var courses = await _mediator.Send(query);
        return Ok(courses);
    }

    [HttpGet("popular")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetPopularCourses()
    {
        var query = new GetPopularCoursesQuery { Count = 10 };
        var courses = await _mediator.Send(query);
        return Ok(courses);
    }

    [HttpGet("categories")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        var query = new GetCategoriesQuery();
        var categories = await _mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CourseDto>>> SearchCourses([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var searchQuery = new SearchCoursesQuery
        {
            Query = query,
            Page = page,
            PageSize = pageSize
        };

        var courses = await _mediator.Send(searchQuery);
        return Ok(courses);
    }

    [HttpGet("my-courses")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetMyCourses()
    {
        var userId = GetCurrentUserId();
        var query = new GetCoursesByInstructorQuery { InstructorId = userId };
        var courses = await _mediator.Send(query);
        return Ok(courses);
    }

    [HttpGet("enrolled")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetEnrolledCourses()
    {
        var userId = GetCurrentUserId();
        var query = new GetEnrolledCoursesQuery { UserId = userId };
        var courses = await _mediator.Send(query);
        return Ok(courses);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException());
    }
}

public class TranscribeVideoRequest
{
    public string VideoUrl { get; set; } = string.Empty;
}