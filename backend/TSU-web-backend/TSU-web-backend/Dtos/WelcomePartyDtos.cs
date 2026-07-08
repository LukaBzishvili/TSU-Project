using System.ComponentModel.DataAnnotations;

namespace TSU_web_backend.Dtos;

public class WelcomePartyRegistrationRequest
{
    [Required, MinLength(2), StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(4), StringLength(30)]
    public string StudentId { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(120)]
    public string Faculty { get; set; } = string.Empty;

    [Range(0, 2)]
    public int GuestCount { get; set; }

    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;

    public bool Agreed { get; set; }
}

public record WelcomePartyRegistrationResponse(
    int Id,
    string FullName,
    string Email,
    string StudentId,
    string Faculty,
    int GuestCount,
    string Notes,
    string TicketCode,
    string Status,
    DateTime RegisteredAt);
