namespace WorkForce360.API.Models;

public class AppUser
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = UserRoles.Employee;

    public DateTime CreatedAtUtc { get; set; }
}
