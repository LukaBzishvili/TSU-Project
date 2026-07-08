namespace TSU_web_backend.Models;

public class WelcomePartyRegistration
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string Faculty { get; set; } = string.Empty;
    public int GuestCount { get; set; }
    public string Notes { get; set; } = string.Empty;
    public bool AgreedToTerms { get; set; }
    public string TicketCode { get; set; } = string.Empty;
    public string Status { get; set; } = "Registered";
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}
