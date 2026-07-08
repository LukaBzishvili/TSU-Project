namespace TSU_web_backend.Models;

public class StudentExperience
{
    public int Id { get; set; }
    public int StudentProfileId { get; set; }
    public StudentProfile StudentProfile { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
