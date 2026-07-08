using System.ComponentModel.DataAnnotations;

namespace TSU_web_backend.Dtos;

public class StudentExperienceRequest
{
    public StudentExperienceRequest()
    {
    }

    public StudentExperienceRequest(string title, string organization, string period, string description)
    {
        Title = title;
        Organization = organization;
        Period = period;
        Description = description;
    }

    [StringLength(80)]
    public string Title { get; set; } = string.Empty;

    [StringLength(120)]
    public string Organization { get; set; } = string.Empty;

    [StringLength(40)]
    public string Period { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}

public class UpdateStudentProfileRequest
{
    [Required, MinLength(2), StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, MinLength(4), StringLength(30)]
    public string StudentIdNumber { get; set; } = string.Empty;

    [Required, MinLength(2), StringLength(120)]
    public string Department { get; set; } = string.Empty;

    [Range(2020, 2100)]
    public int GraduationYear { get; set; }

    [Required, MinLength(10), StringLength(1500)]
    public string Summary { get; set; } = string.Empty;

    [Url, StringLength(250)]
    public string? LinkedInUrl { get; set; }

    [Url, StringLength(250)]
    public string? GitHubUrl { get; set; }

    [Url, StringLength(250)]
    public string? PortfolioUrl { get; set; }

    [Phone, StringLength(25)]
    public string? Phone { get; set; }

    public bool IsVisibleToHr { get; set; }

    [MaxLength(25)]
    public List<string> Skills { get; set; } = [];

    [MaxLength(15)]
    public List<StudentExperienceRequest> Experiences { get; set; } = [];
}

public record StudentProfileResponse(
    int Id,
    string Email,
    string FirstName,
    string LastName,
    string StudentIdNumber,
    string Department,
    int GraduationYear,
    string Summary,
    string? LinkedInUrl,
    string? GitHubUrl,
    string? PortfolioUrl,
    string? Phone,
    bool IsVisibleToHr,
    IEnumerable<string> Skills,
    IEnumerable<StudentExperienceRequest> Experiences,
    IEnumerable<CvFileResponse> CvFiles);

public record CvFileResponse(
    int Id,
    string OriginalFileName,
    string StoredFileName,
    string FilePath,
    DateTime UploadedAt);
