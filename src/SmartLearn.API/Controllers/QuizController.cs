using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLearn.Application.Commands.Quiz;
using SmartLearn.Application.DTOs.Quiz;

namespace SmartLearn.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuizController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuizController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<GeneratedQuizDto>> GenerateQuiz(GenerateQuizCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}