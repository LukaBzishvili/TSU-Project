using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSU_web_backend.Data;
using TSU_web_backend.Dtos;
using TSU_web_backend.Models;

namespace TSU_web_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WelcomePartyController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<WelcomePartyRegistrationResponse>> Register(WelcomePartyRegistrationRequest request)
    {
        if (!request.Agreed)
        {
            return BadRequest("You must confirm that the registration information is correct.");
        }

        if (request.GuestCount is < 0 or > 2)
        {
            return BadRequest("Guest count must be between 0 and 2.");
        }

        var ticketCode = await GenerateUniqueTicketCodeAsync();

        var registration = new WelcomePartyRegistration
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            StudentId = request.StudentId.Trim(),
            Faculty = request.Faculty.Trim(),
            GuestCount = request.GuestCount,
            Notes = request.Notes.Trim(),
            AgreedToTerms = request.Agreed,
            TicketCode = ticketCode
        };

        dbContext.WelcomePartyRegistrations.Add(registration);
        await dbContext.SaveChangesAsync();

        return Ok(MapResponse(registration));
    }

    [HttpGet("{ticketCode}")]
    public async Task<ActionResult<WelcomePartyRegistrationResponse>> GetByTicketCode(string ticketCode)
    {
        var registration = await dbContext.WelcomePartyRegistrations
            .FirstOrDefaultAsync(item => item.TicketCode == ticketCode);

        return registration is null ? NotFound() : Ok(MapResponse(registration));
    }

    private async Task<string> GenerateUniqueTicketCodeAsync()
    {
        string code;

        do
        {
            code = $"TSU-WP-{Random.Shared.Next(100000, 999999)}";
        } while (await dbContext.WelcomePartyRegistrations.AnyAsync(item => item.TicketCode == code));

        return code;
    }

    private static WelcomePartyRegistrationResponse MapResponse(WelcomePartyRegistration registration)
    {
        return new WelcomePartyRegistrationResponse(
            registration.Id,
            registration.FullName,
            registration.Email,
            registration.StudentId,
            registration.Faculty,
            registration.GuestCount,
            registration.Notes,
            registration.TicketCode,
            registration.Status,
            registration.RegisteredAt);
    }
}
