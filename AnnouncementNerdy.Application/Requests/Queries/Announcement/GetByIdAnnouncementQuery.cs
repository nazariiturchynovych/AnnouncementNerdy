using Microsoft.Extensions.Logging;

namespace AnnouncementNerdy.Application.Requests.Queries.Announcement;

using Repositories;
using Domain.Results;
using FluentValidation;
using AnnouncementNerdy.Domain.Entities.Announcement;

public record GetByIdAnnouncementQuery(string Id) : IRequest<CommonResult<Announcement>>;

public class GetByIdAnnouncementQueryHandler : IRequestHandler<GetByIdAnnouncementQuery, CommonResult<Announcement>>
{
    private readonly IAnnouncementRepository _announcementRepository;
    private ILogger<GetByIdAnnouncementQueryHandler> _logger;

    public GetByIdAnnouncementQueryHandler(IAnnouncementRepository announcementRepository, ILogger<GetByIdAnnouncementQueryHandler> logger)
    {
        _announcementRepository = announcementRepository;
        _logger = logger;
    }
    
    public async Task<CommonResult<Announcement>> Handle(GetByIdAnnouncementQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var announcement = await _announcementRepository.GetByIdAsync(request.Id);

            if (announcement is null)
            {
                return Failure<Announcement>("Entity does not exist");
            }

            return Success(announcement);
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong in {@Handler}, Error:{@Error}", nameof(GetByIdAnnouncementQueryHandler), e.Message);
            throw;
        }
        
    }
}

public class GetByIdAnnouncementQueryValidator : AbstractValidator<GetByIdAnnouncementQuery>
{
    public GetByIdAnnouncementQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}