namespace WorkForce360.API.Models;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string HR = "HR";
    public const string Manager = "Manager";
    public const string Employee = "Employee";
    public const string Recruiter = "Recruiter";

    private static readonly string[] ValidRoles = [Admin, HR, Manager, Employee, Recruiter];

    public static bool IsValid(string role) => ValidRoles.Contains(role, StringComparer.OrdinalIgnoreCase);

    public static string Normalize(string role) => ValidRoles.Single(
        validRole => validRole.Equals(role, StringComparison.OrdinalIgnoreCase));
}
