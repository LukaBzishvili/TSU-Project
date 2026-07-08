using System.ComponentModel.DataAnnotations;

namespace TSU_web_backend.Dtos;

public class RegisterStudentRequest
{
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8), StringLength(64)]
    public string Password { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, MinLength(4), StringLength(30)]
    public string StudentIdNumber { get; set; } = string.Empty;

    [Range(2020, 2100)]
    public int GraduationYear { get; set; }
}

public class RegisterHrRequest
{
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8), StringLength(64)]
    public string Password { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(120)]
    public string CompanyName { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(80)]
    public string Position { get; set; } = string.Empty;

    [Required, Phone, StringLength(25)]
    public string ContactPhone { get; set; } = string.Empty;
}

public class RegisterSelfGovernmentRequest
{
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8), StringLength(64)]
    public string Password { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(120)]
    public string OrganizationName { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(80)]
    public string RepresentativeName { get; set; } = string.Empty;
}

public class LoginRequest
{
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8), StringLength(64)]
    public string Password { get; set; } = string.Empty;
}

public record AuthResponse(string Token, string Email, string Role, string Message);
