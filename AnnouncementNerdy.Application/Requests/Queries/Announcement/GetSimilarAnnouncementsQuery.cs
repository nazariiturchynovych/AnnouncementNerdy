using AnnouncementNerdy.Domain.Enums;

namespace AnnouncementNerdy.Application.Requests.Queries.Announcement;

using Repositories;
using Domain.Results;
using FluentValidation;
using AnnouncementNerdy.Domain.Entities.Announcement;

public record GetSimilarAnnouncementsQuery(string Id, OrderBy OrderBy) : IRequest<CommonResult<IOrderedEnumerable<Announcement>>>;

public class
    GetSimilarAnnouncementsQueryHandler : IRequestHandler<GetSimilarAnnouncementsQuery,
    CommonResult<IOrderedEnumerable<Announcement>>>
{
    private readonly IAnnouncementRepository _announcementRepository;

    public GetSimilarAnnouncementsQueryHandler(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }

    public async Task<CommonResult<IOrderedEnumerable<Announcement>>> Handle(GetSimilarAnnouncementsQuery request,
        CancellationToken cancellationToken)
    {
        var announcements = await _announcementRepository.GetSimilar(request.Id);

        if (!announcements.Any())
        {
            return Failure<IOrderedEnumerable<Announcement>>("There is not simmilar announcements");
        }
        

        var orderedAnnouncements = request.OrderBy switch
        {
            OrderBy.Ascending => announcements.OrderBy(x => x.CreatedDate),
            OrderBy.Descending => announcements.OrderByDescending(x => x.CreatedDate)
        };
        

        return Success(orderedAnnouncements!);
    }
}

public class GetSimilarAnnouncementsQueryValidator : AbstractValidator<GetSimilarAnnouncementsQuery>
{
    public GetSimilarAnnouncementsQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}