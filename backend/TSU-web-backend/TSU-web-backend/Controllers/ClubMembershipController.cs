using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSU_web_backend.Data;
using TSU_web_backend.Dtos;
using TSU_web_backend.Models;

namespace TSU_web_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubMembershipController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPost("apply")]
    public async Task<ActionResult<ClubMembershipApplicationResponse>> Apply(
        ClubMembershipApplicationRequest request)
    {
        if (!request.AgreedToContact)
        {
            return BadRequest("You must agree to be contacted about club membership.");
        }

        var duplicateExists = await dbContext.ClubMembershipApplications.AnyAsync(item =>
            item.Email == request.Email.Trim().ToLowerInvariant()
            && item.ClubName == request.ClubName.Trim()
            && item.Status != "Archived");

        if (duplicateExists)
        {
            return BadRequest("An active application for this club already exists with this email.");
        }

        var application = new ClubMembershipApplication
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            StudentId = request.StudentId.Trim(),
            ClubName = request.ClubName.Trim(),
            InterestArea = request.InterestArea.Trim(),
            Motivation = request.Motivation.Trim(),
            AgreedToContact = request.AgreedToContact,
            ApplicationCode = await GenerateUniqueApplicationCodeAsync()
        };

        dbContext.ClubMembershipApplications.Add(application);
        await dbContext.SaveChangesAsync();

        return Ok(MapResponse(application));
    }

    [HttpGet("{applicationCode}")]
    public async Task<ActionResult<ClubMembershipApplicationResponse>> GetByCode(string applicationCode)
    {
        var application = await dbContext.ClubMembershipApplications
            .FirstOrDefaultAsync(item => item.ApplicationCode == applicationCode);

        return application is null ? NotFound() : Ok(MapResponse(application));
    }

    private async Task<string> GenerateUniqueApplicationCodeAsync()
    {
        string code;

        do
        {
            code = $"TSU-CLUB-{Random.Shared.Next(100000, 999999)}";
        } while (await dbContext.ClubMembershipApplications.AnyAsync(item => item.ApplicationCode == code));

        return code;
    }

    private static ClubMembershipApplicationResponse MapResponse(ClubMembershipApplication application)
    {
        return new ClubMembershipApplicationResponse(
            application.Id,
            application.FullName,
            application.Email,
            application.StudentId,
            application.ClubName,
            application.InterestArea,
            application.Motivation,
            application.ApplicationCode,
            application.Status,
            application.AppliedAt);
    }
}
