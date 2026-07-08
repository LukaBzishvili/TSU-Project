using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSU_web_backend.Data;
using TSU_web_backend.Dtos;
using TSU_web_backend.Models;
using TSU_web_backend.Services;

namespace TSU_web_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    ApplicationDbContext dbContext,
    ITokenService tokenService) : ControllerBase
{
    private readonly PasswordHasher<AppUser> _passwordHasher = new();

    [HttpPost("register/student")]
    public async Task<ActionResult<AuthResponse>> RegisterStudent(RegisterStudentRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        if (await dbContext.Users.AnyAsync(user => user.Email == normalizedEmail))
        {
            return Conflict("A user with this email already exists.");
        }

        var user = new AppUser
        {
            Email = normalizedEmail,
            Role = UserRole.Student
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        user.StudentProfile = new StudentProfile
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            StudentIdNumber = request.StudentIdNumber.Trim(),
            GraduationYear = request.GraduationYear
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var token = tokenService.CreateToken(user);
        return Ok(new AuthResponse(token, user.Email, user.Role.ToString(), "Student account created."));
    }

    [HttpPost("register/hr")]
    public async Task<ActionResult<AuthResponse>> RegisterHr(RegisterHrRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        if (await dbContext.Users.AnyAsync(user => user.Email == normalizedEmail))
        {
            return Conflict("A user with this email already exists.");
        }

        var user = new AppUser
        {
            Email = normalizedEmail,
            Role = UserRole.HR
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        user.HrProfile = new HrProfile
        {
            CompanyName = request.CompanyName.Trim(),
            Position = request.Position.Trim(),
            ContactPhone = request.ContactPhone.Trim(),
            IsApproved = false
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var token = tokenService.CreateToken(user);
        return Ok(new AuthResponse(token, user.Email, user.Role.ToString(), "HR account created and awaiting approval."));
    }

    [HttpPost("register/self-government")]
    public async Task<ActionResult<AuthResponse>> RegisterSelfGovernment(RegisterSelfGovernmentRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        if (await dbContext.Users.AnyAsync(user => user.Email == normalizedEmail))
        {
            return Conflict("A user with this email already exists.");
        }

        var user = new AppUser
        {
            Email = normalizedEmail,
            Role = UserRole.SelfGovernment
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var token = tokenService.CreateToken(user);
        return Ok(new AuthResponse(token, user.Email, user.Role.ToString(), "Self-government account created."));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await dbContext.Users
            .Include(appUser => appUser.HrProfile)
            .FirstOrDefaultAsync(appUser => appUser.Email == normalizedEmail);

        if (user is null)
        {
            return Unauthorized("Invalid email or password.");
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid email or password.");
        }

        var message = user.Role == UserRole.HR && user.HrProfile is { IsApproved: false }
            ? "HR account exists but still needs approval."
            : "Login successful.";

        var token = tokenService.CreateToken(user);
        return Ok(new AuthResponse(token, user.Email, user.Role.ToString(), message));
    }
}
