namespace AnnouncementNerdy.Application.Requests.Queries.Announcement;


using Repositories;
using Domain.Results;
using FluentValidation;
using AnnouncementNerdy.Domain.Entities.Announcement;

public record GetAnnouncementListQuery() : IRequest<CommonResult<IEnumerable<Announcement>>>;

public class GetAnnouncementListQueryHandler : IRequestHandler<GetAnnouncementListQuery, CommonResult<IEnumerable<Announcement>>>
{
    private readonly IAnnouncementRepository _announcementRepository;

    public GetAnnouncementListQueryHandler(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }
    
    public async Task<CommonResult<IEnumerable<Announcement>>> Handle(GetAnnouncementListQuery request, CancellationToken cancellationToken)
    {
        var announcements = await _announcementRepository.GetListAsync();

        if (!announcements.Any())
        {
            return Failure<IEnumerable<Announcement>>("There is no announcements");
        }

        return Success(announcements);
    }
}

public class GetAnnouncementListQueryValidator : AbstractValidator<GetAnnouncementListQuery>
{
}