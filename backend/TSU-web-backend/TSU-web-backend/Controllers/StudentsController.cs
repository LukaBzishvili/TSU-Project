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
[Authorize(Roles = nameof(UserRole.Student))]
public class StudentsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<StudentProfileResponse>> GetMyProfile()
    {
        var userId = GetCurrentUserId();
        var profile = await dbContext.StudentProfiles
            .Include(studentProfile => studentProfile.User)
            .Include(studentProfile => studentProfile.StudentSkills)
                .ThenInclude(studentSkill => studentSkill.Skill)
            .Include(studentProfile => studentProfile.Experiences)
            .Include(studentProfile => studentProfile.CvFiles)
            .FirstOrDefaultAsync(studentProfile => studentProfile.UserId == userId);

        return profile is null ? NotFound() : Ok(MapStudentProfile(profile));
    }

    [HttpPut("me")]
    public async Task<ActionResult<StudentProfileResponse>> UpdateMyProfile(UpdateStudentProfileRequest request)
    {
        var userId = GetCurrentUserId();
        var profile = await dbContext.StudentProfiles
            .Include(studentProfile => studentProfile.StudentSkills)
                .ThenInclude(studentSkill => studentSkill.Skill)
            .Include(studentProfile => studentProfile.Experiences)
            .Include(studentProfile => studentProfile.CvFiles)
            .Include(studentProfile => studentProfile.User)
            .FirstOrDefaultAsync(studentProfile => studentProfile.UserId == userId);

        if (profile is null)
        {
            return NotFound();
        }

        profile.FirstName = request.FirstName.Trim();
        profile.LastName = request.LastName.Trim();
        profile.StudentIdNumber = request.StudentIdNumber.Trim();
        profile.Department = request.Department.Trim();
        profile.GraduationYear = request.GraduationYear;
        profile.Summary = request.Summary.Trim();
        profile.LinkedInUrl = request.LinkedInUrl?.Trim();
        profile.GitHubUrl = request.GitHubUrl?.Trim();
        profile.PortfolioUrl = request.PortfolioUrl?.Trim();
        profile.Phone = request.Phone?.Trim();
        profile.IsVisibleToHr = request.IsVisibleToHr;
        profile.UpdatedAt = DateTime.UtcNow;

        dbContext.StudentExperiences.RemoveRange(profile.Experiences);
        profile.Experiences = request.Experiences
            .Select(experience => new StudentExperience
            {
                Title = experience.Title.Trim(),
                Organization = experience.Organization.Trim(),
                Period = experience.Period.Trim(),
                Description = experience.Description.Trim()
            })
            .ToList();

        dbContext.StudentSkills.RemoveRange(profile.StudentSkills);
        profile.StudentSkills = [];

        foreach (var skillName in request.Skills
                     .Select(skill => skill.Trim())
                     .Where(skill => !string.IsNullOrWhiteSpace(skill))
                     .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var existingSkill = await dbContext.Skills.FirstOrDefaultAsync(skill =>
                skill.Name.ToLower() == skillName.ToLower());

            var skill = existingSkill ?? new Skill { Name = skillName };
            if (existingSkill is null)
            {
                dbContext.Skills.Add(skill);
            }

            profile.StudentSkills.Add(new StudentSkill
            {
                StudentProfile = profile,
                Skill = skill
            });
        }

        await dbContext.SaveChangesAsync();

        var refreshedProfile = await dbContext.StudentProfiles
            .Include(studentProfile => studentProfile.User)
            .Include(studentProfile => studentProfile.StudentSkills)
                .ThenInclude(studentSkill => studentSkill.Skill)
            .Include(studentProfile => studentProfile.Experiences)
            .Include(studentProfile => studentProfile.CvFiles)
            .FirstAsync(studentProfile => studentProfile.Id == profile.Id);

        return Ok(MapStudentProfile(refreshedProfile));
    }

    [HttpPost("me/cv")]
    [RequestSizeLimit(10_000_000)]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<CvFileResponse>> UploadCv(IFormFile file)
    {
        if (file.Length == 0)
        {
            return BadRequest("Please upload a non-empty file.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest("Only PDF, DOC, and DOCX files are allowed.");
        }

        var userId = GetCurrentUserId();
        var profile = await dbContext.StudentProfiles.FirstOrDefaultAsync(studentProfile => studentProfile.UserId == userId);
        if (profile is null)
        {
            return NotFound("Student profile was not found.");
        }

        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "CVs");
        Directory.CreateDirectory(uploadsPath);

        var storedFileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(uploadsPath, storedFileName);

        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var cvFile = new CvFile
        {
            StudentProfileId = profile.Id,
            OriginalFileName = file.FileName,
            StoredFileName = storedFileName,
            FilePath = fullPath
        };

        dbContext.CvFiles.Add(cvFile);
        await dbContext.SaveChangesAsync();

        return Ok(new CvFileResponse(
            cvFile.Id,
            cvFile.OriginalFileName,
            cvFile.StoredFileName,
            cvFile.FilePath,
            cvFile.UploadedAt));
    }

    private int GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out var parsedUserId))
        {
            throw new InvalidOperationException("User identifier is missing from the token.");
        }

        return parsedUserId;
    }

    private static StudentProfileResponse MapStudentProfile(StudentProfile profile)
    {
        return new StudentProfileResponse(
            profile.Id,
            profile.User.Email,
            profile.FirstName,
            profile.LastName,
            profile.StudentIdNumber,
            profile.Department,
            profile.GraduationYear,
            profile.Summary,
            profile.LinkedInUrl,
            profile.GitHubUrl,
            profile.PortfolioUrl,
            profile.Phone,
            profile.IsVisibleToHr,
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
                file.UploadedAt)));
    }
}
