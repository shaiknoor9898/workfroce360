using System.ComponentModel.DataAnnotations;

namespace WorkForce360.API.DTOs;

public class InitializeAdminRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{12,128}$", ErrorMessage = "Password must be 12 to 128 characters and include uppercase, lowercase, and a number.")]
    public string Password { get; set; } = string.Empty;
}

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(128)]
    public string Password { get; set; } = string.Empty;
}

public class CreateUserRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{12,128}$", ErrorMessage = "Password must be 12 to 128 characters and include uppercase, lowercase, and a number.")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Role { get; set; } = string.Empty;
}

public class AuthTokenResponseDto
{
    public string AccessToken { get; init; } = string.Empty;

    public string TokenType { get; init; } = "Bearer";

    public DateTime ExpiresAtUtc { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;
}

public class UserResponseDto
{
    public int Id { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;

    public DateTime CreatedAtUtc { get; init; }
}
