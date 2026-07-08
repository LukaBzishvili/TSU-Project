using System.ComponentModel.DataAnnotations;

namespace TSU_web_backend.Dtos;

public record EventResponse(
    int Id,
    string Title,
    string Date,
    string Time,
    string Location,
    string Contact,
    string Format,
    string Summary,
    string Organizer);

public class CreateEventRequest
{
    [Required, MinLength(3), StringLength(160)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateOnly Date { get; set; }

    [Required, MinLength(2), StringLength(40)]
    public string Time { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(180)]
    public string Location { get; set; } = string.Empty;

    [Required, MinLength(5), StringLength(150)]
    public string Contact { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(60)]
    public string Format { get; set; } = string.Empty;

    [Required, MinLength(10), StringLength(1000)]
    public string Summary { get; set; } = string.Empty;

    [StringLength(120)]
    public string Organizer { get; set; } = "Self-government";
}
