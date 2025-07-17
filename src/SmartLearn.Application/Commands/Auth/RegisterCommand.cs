using MediatR;
using SmartLearn.Application.DTOs.Auth;

namespace SmartLearn.Application.Commands.Auth;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}