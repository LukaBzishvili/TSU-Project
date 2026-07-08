using System.ComponentModel.DataAnnotations;

namespace TSU_web_backend.Dtos;

public record HrStudentCardResponse(
    int ProfileId,
    string FullName,
    string Email,
    string Department,
    int GraduationYear,
    string Summary,
    IEnumerable<string> Skills,
    IEnumerable<StudentExperienceRequest> Experiences,
    IEnumerable<CvFileResponse> CvFiles);

public class ApproveHrRequest
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    public bool IsApproved { get; set; }
}
