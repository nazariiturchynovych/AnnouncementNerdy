using Microsoft.Extensions.Logging;

namespace AnnouncementNerdy.Application.Requests.Queries.Announcement;


using Repositories;
using Domain.Results;
using FluentValidation;
using AnnouncementNerdy.Domain.Entities.Announcement;

public record GetAnnouncementListQuery() : IRequest<CommonResult<IEnumerable<Announcement>>>;

public class GetAnnouncementListQueryHandler : IRequestHandler<GetAnnouncementListQuery, CommonResult<IEnumerable<Announcement>>>
{
    private readonly IAnnouncementRepository _announcementRepository;
    private ILogger<GetAnnouncementListQueryHandler> _logger;

    public GetAnnouncementListQueryHandler(IAnnouncementRepository announcementRepository, ILogger<GetAnnouncementListQueryHandler> logger)
    {
        _announcementRepository = announcementRepository;
        _logger = logger;
    }
    
    public async Task<CommonResult<IEnumerable<Announcement>>> Handle(GetAnnouncementListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var announcements = await _announcementRepository.GetListAsync();

            if (!announcements.Any())
            {
                return Failure<IEnumerable<Announcement>>("There is no announcements");
            }

            return Success(announcements);
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong in {@Handler}, Error:{@Error}", nameof(GetAnnouncementListQueryHandler), e.Message);
            throw;
        }
        
    }
}

public class GetAnnouncementListQueryValidator : AbstractValidator<GetAnnouncementListQuery>
{
}