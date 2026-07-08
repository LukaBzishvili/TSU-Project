namespace TSU_web_backend.Models;

public class HrProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public string CompanyName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}
