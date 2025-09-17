namespace ReactASP.Application.DTOs.RegisterUser;

public sealed class RegisterRequest
{
    public string Email { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string Password { get; set; } = default!;
}