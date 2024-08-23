using AnnouncementNerdy.Application.Requests.Commands.Announcement;
using AnnouncementNerdy.Application.Requests.Queries.Announcement;
using AnnouncementNerdy.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncementNerdy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnnouncementsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    

    [HttpPost]
    public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementCommand command, ILogger<AnnouncementsController> logger)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnnouncement(string id)
    {
        return Ok(await _mediator.Send(new DeleteAnnouncementCommand(id)));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnnouncement(UpdateAnnouncementCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        return Ok(await _mediator.Send(new GetAnnouncementByIdQuery(id)));
    }
    
    
    [HttpGet("list")]
    public async Task<IActionResult> GetAnnouncementList()
    {
        return Ok(await _mediator.Send(new GetAnnouncementListQuery()));
    }

    [HttpGet("similar/{id}")]
    public async Task<IActionResult> GetSimilarAnnouncements(string id, OrderBy orderBy)
    {
        return Ok(await _mediator.Send(new GetSimilarAnnouncementsQuery(id, orderBy)));
    }
}