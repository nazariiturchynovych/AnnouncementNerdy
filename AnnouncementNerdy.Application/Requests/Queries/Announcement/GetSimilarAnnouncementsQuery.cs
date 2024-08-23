using AnnouncementNerdy.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace AnnouncementNerdy.Application.Requests.Queries.Announcement;

using Repositories;
using Domain.Results;
using FluentValidation;
using AnnouncementNerdy.Domain.Entities.Announcement;

public record GetSimilarAnnouncementsQuery(string Id, OrderBy OrderBy) : IRequest<CommonResult<List<Announcement>>>;

public class
    GetSimilarAnnouncementsQueryHandler : IRequestHandler<GetSimilarAnnouncementsQuery,
    CommonResult<List<Announcement>>>
{
    private readonly IAnnouncementRepository _announcementRepository;
    private ILogger<GetSimilarAnnouncementsQueryHandler> _logger;

    public GetSimilarAnnouncementsQueryHandler(IAnnouncementRepository announcementRepository, ILogger<GetSimilarAnnouncementsQueryHandler> logger)
    {
        _announcementRepository = announcementRepository;
        _logger = logger;
    }

    public async Task<CommonResult<List<Announcement>>> Handle(GetSimilarAnnouncementsQuery request,
        CancellationToken cancellationToken)
    {

        try
        {
            var announcements = await _announcementRepository.GetSimilar(request.Id);

            if (!announcements.Any())
            {
                return Failure<List<Announcement>>("There is not similar announcements");
            }
        

            var orderedAnnouncements = request.OrderBy switch
            {
                OrderBy.Ascending => announcements.OrderBy(x => x.CreatedDate),
                OrderBy.Descending => announcements.OrderByDescending(x => x.CreatedDate)
            };
        

            return Success(orderedAnnouncements.ToList()!);
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong in {@Handler}, Error:{@Error}", nameof(GetSimilarAnnouncementsQueryHandler), e.Message);
            throw;
        }
        
    }
}

public class GetSimilarAnnouncementsQueryValidator : AbstractValidator<GetSimilarAnnouncementsQuery>
{
    public GetSimilarAnnouncementsQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}