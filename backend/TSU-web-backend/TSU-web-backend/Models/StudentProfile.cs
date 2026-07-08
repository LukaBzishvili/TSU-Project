namespace TSU_web_backend.Models;

public class StudentProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string StudentIdNumber { get; set; } = string.Empty;
    public string Department { get; set; } = "Computer Science";
    public int GraduationYear { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? PortfolioUrl { get; set; }
    public string? Phone { get; set; }
    public bool IsVisibleToHr { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<StudentExperience> Experiences { get; set; } = [];
    public ICollection<StudentSkill> StudentSkills { get; set; } = [];
    public ICollection<CvFile> CvFiles { get; set; } = [];
}
