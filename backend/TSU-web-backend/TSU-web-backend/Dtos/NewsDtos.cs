namespace TSU_web_backend.Dtos;

public class ExternalNewsItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Category { get; set; } = "TSU CS";
    public string Audience { get; set; } = "Students";
    public string Url { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Source { get; set; } = "TSU Computer Science";
}
