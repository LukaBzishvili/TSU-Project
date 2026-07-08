namespace TSU_web_backend.Models;

public class ClubMembershipApplication
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string ClubName { get; set; } = string.Empty;
    public string InterestArea { get; set; } = string.Empty;
    public string Motivation { get; set; } = string.Empty;
    public bool AgreedToContact { get; set; }
    public string ApplicationCode { get; set; } = string.Empty;
    public string Status { get; set; } = "Submitted";
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}
