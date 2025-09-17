using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactASP.Api.Contracts.Auth;
using ReactASP.Application.Auth.Commands.RegisterUser;

namespace ReactASP.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _meditator;
    public AuthController(ISender meditator)
    {
        _meditator = meditator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var cmd = new RegisterUserCommand(request.Email, request.DisplayName, request.Password);

        try
        {
            var result = await _meditator.Send(cmd, ct);

            var resp = new RegisterResponse
            {
                UserId = result.UserId,
                Email = result.Email,
                DisplayName = result.DisplayName,
                Role = result.Role
            };

            return CreatedAtAction(nameof(Register), new { Id = resp.UserId }, resp);
        } catch (InvalidOperationException ex)
        {
            return ValidationProblem(new ValidationProblemDetails
            {
                Title = "Registration Error",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /*[HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]*/
}