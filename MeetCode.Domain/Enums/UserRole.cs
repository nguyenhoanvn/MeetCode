namespace MeetCode.Domain.Enums;

public static class UserRole {
    public const string Admin = "admin";
    public const string Moderator = "moderator";
    public const string User = "user";

    public static bool IsValid(string role) => role is Admin or Moderator or User;
}