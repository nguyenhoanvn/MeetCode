namespace ReactASP.Application.DTOs.LoginUser
{
    public sealed class LoginResponse
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
