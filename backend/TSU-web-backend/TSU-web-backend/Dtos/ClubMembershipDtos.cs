using System.ComponentModel.DataAnnotations;

namespace TSU_web_backend.Dtos;

public class ClubMembershipApplicationRequest
{
    [Required, MinLength(2), StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(4), StringLength(30)]
    public string StudentId { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(100)]
    public string ClubName { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(100)]
    public string InterestArea { get; set; } = string.Empty;

    [Required, MinLength(20), StringLength(700)]
    public string Motivation { get; set; } = string.Empty;

    public bool AgreedToContact { get; set; }
}

public record ClubMembershipApplicationResponse(
    int Id,
    string FullName,
    string Email,
    string StudentId,
    string ClubName,
    string InterestArea,
    string Motivation,
    string ApplicationCode,
    string Status,
    DateTime AppliedAt);
