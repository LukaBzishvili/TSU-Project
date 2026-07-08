using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSU_web_backend.Data;
using TSU_web_backend.Dtos;
using TSU_web_backend.Models;

namespace TSU_web_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetEvents()
    {
        var events = await dbContext.Events
            .OrderBy(eventItem => eventItem.Date)
            .ThenBy(eventItem => eventItem.Time)
            .ToListAsync();

        return Ok(events.Select(ToResponse));
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRole.SelfGovernment)},{nameof(UserRole.Admin)}")]
    public async Task<ActionResult<EventResponse>> CreateEvent(CreateEventRequest request)
    {
        var eventItem = new Event
        {
            Title = request.Title.Trim(),
            Date = request.Date,
            Time = request.Time.Trim(),
            Location = request.Location.Trim(),
            Contact = request.Contact.Trim(),
            Format = request.Format.Trim(),
            Summary = request.Summary.Trim(),
            Organizer = string.IsNullOrWhiteSpace(request.Organizer)
                ? "Self-government"
                : request.Organizer.Trim()
        };

        dbContext.Events.Add(eventItem);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvents), new { id = eventItem.Id }, ToResponse(eventItem));
    }

    private static EventResponse ToResponse(Event eventItem)
    {
        return new EventResponse(
            eventItem.Id,
            eventItem.Title,
            eventItem.Date.ToString("yyyy-MM-dd"),
            eventItem.Time,
            eventItem.Location,
            eventItem.Contact,
            eventItem.Format,
            eventItem.Summary,
            eventItem.Organizer);
    }
}
