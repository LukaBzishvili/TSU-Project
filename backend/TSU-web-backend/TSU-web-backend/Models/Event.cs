namespace TSU_web_backend.Models;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Time { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Organizer { get; set; } = "Self-government";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
