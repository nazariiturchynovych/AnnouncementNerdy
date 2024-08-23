using AnnouncementNerdy.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace AnnouncementNerdy.Application.Requests.Queries.Announcement;

using Repositories;
using Domain.Results;
using FluentValidation;
using AnnouncementNerdy.Domain.Entities.Announcement;

public record GetAnnouncementByIdQuery(string Id) : IRequest<CommonResult<Announcement>>;

public class GetAnnouncementByIdQueryHandler : IRequestHandler<GetAnnouncementByIdQuery, CommonResult<Announcement>>
{
    private readonly IAnnouncementRepository _announcementRepository;
    private ILogger<GetAnnouncementByIdQueryHandler> _logger;

    public GetAnnouncementByIdQueryHandler(IAnnouncementRepository announcementRepository, ILogger<GetAnnouncementByIdQueryHandler> logger)
    {
        _announcementRepository = announcementRepository;
        _logger = logger;
    }
    
    public async Task<CommonResult<Announcement>> Handle(GetAnnouncementByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var announcement = await _announcementRepository.GetByIdAsync(request.Id);

            if (announcement is null)
            {
                return Failure<Announcement>(CommonErrors.EntityDoesNotExist);
            }

            return Success(announcement);
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong in {@Handler}, Error:{@Error}", nameof(GetAnnouncementByIdQueryHandler), e.Message);
            throw;
        }
        
    }
}

public class GetByIdAnnouncementQueryValidator : AbstractValidator<GetAnnouncementByIdQuery>
{
    public GetByIdAnnouncementQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}