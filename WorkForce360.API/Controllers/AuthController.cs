using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkForce360.API.Data;
using WorkForce360.API.DTOs;
using WorkForce360.API.Models;
using WorkForce360.API.Services;

namespace WorkForce360.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    WorkForce360DbContext context,
    PasswordHashingService passwordHashing,
    JwtTokenService jwtTokenService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("initialize-admin")]
    public async Task<ActionResult<AuthTokenResponseDto>> InitializeAdmin(InitializeAdminRequestDto request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        if (await context.Users.AnyAsync())
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "An administrator has already been initialized."
            });
        }

        var admin = new AppUser
        {
            Email = NormalizeEmail(request.Email),
            PasswordHash = passwordHashing.Hash(request.Password),
            Role = UserRoles.Admin,
            CreatedAtUtc = DateTime.UtcNow
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return Ok(jwtTokenService.CreateToken(admin));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthTokenResponseDto>> Login(LoginRequestDto request)
    {
        var email = NormalizeEmail(request.Email);
        var user = await context.Users.SingleOrDefaultAsync(existingUser => existingUser.Email == email);
        if (user is null || !passwordHashing.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Invalid email or password."
            });
        }

        return Ok(jwtTokenService.CreateToken(user));
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost("users")]
    public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserRequestDto request)
    {
        if (!UserRoles.IsValid(request.Role))
        {
            ModelState.AddModelError(nameof(request.Role), "Role must be Admin, HR, Manager, Employee, or Recruiter.");
            return ValidationProblem(ModelState);
        }

        var email = NormalizeEmail(request.Email);
        if (await context.Users.AnyAsync(existingUser => existingUser.Email == email))
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "A user with this email already exists."
            });
        }

        var user = new AppUser
        {
            Email = email,
            PasswordHash = passwordHashing.Hash(request.Password),
            Role = UserRoles.Normalize(request.Role),
            CreatedAtUtc = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return StatusCode(StatusCodes.Status201Created, ToResponse(user));
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static UserResponseDto ToResponse(AppUser user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        Role = user.Role,
        CreatedAtUtc = user.CreatedAtUtc
    };
}
