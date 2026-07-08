using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSU_web_backend.Data;
using TSU_web_backend.Dtos;
using TSU_web_backend.Models;

namespace TSU_web_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HrController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet("students")]
    [Authorize(Roles = nameof(UserRole.HR))]
    public async Task<ActionResult<IEnumerable<HrStudentCardResponse>>> GetVisibleStudents()
    {
        var currentHrEmail = User.FindFirstValue(ClaimTypes.Email);
        var hrUser = await dbContext.Users
            .Include(user => user.HrProfile)
            .FirstOrDefaultAsync(user => user.Email == currentHrEmail);

        if (hrUser?.HrProfile?.IsApproved != true)
        {
            return Forbid();
        }

        var students = await dbContext.StudentProfiles
            .Include(profile => profile.User)
            .Include(profile => profile.StudentSkills)
                .ThenInclude(studentSkill => studentSkill.Skill)
            .Include(profile => profile.Experiences)
            .Include(profile => profile.CvFiles)
            .Where(profile => profile.IsVisibleToHr)
            .OrderBy(profile => profile.LastName)
            .ThenBy(profile => profile.FirstName)
            .ToListAsync();

        return Ok(students.Select(profile => new HrStudentCardResponse(
            profile.Id,
            $"{profile.FirstName} {profile.LastName}".Trim(),
            profile.User.Email,
            profile.Department,
            profile.GraduationYear,
            profile.Summary,
            profile.StudentSkills.Select(studentSkill => studentSkill.Skill.Name),
            profile.Experiences.Select(experience => new StudentExperienceRequest(
                experience.Title,
                experience.Organization,
                experience.Period,
                experience.Description)),
            profile.CvFiles.Select(file => new CvFileResponse(
                file.Id,
                file.OriginalFileName,
                file.StoredFileName,
                file.FilePath,
                file.UploadedAt)))));
    }

    [HttpPut("approve")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> ApproveHr(ApproveHrRequest request)
    {
        var hrProfile = await dbContext.HrProfiles.FirstOrDefaultAsync(profile => profile.UserId == request.UserId);
        if (hrProfile is null)
        {
            return NotFound("HR profile was not found.");
        }

        hrProfile.IsApproved = request.IsApproved;
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
