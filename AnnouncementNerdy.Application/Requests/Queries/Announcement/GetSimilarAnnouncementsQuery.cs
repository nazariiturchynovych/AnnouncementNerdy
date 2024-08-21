namespace AnnouncementNerdy.Application.Requests.Queries.Announcement;


using Repositories;
using Domain.Results;
using FluentValidation;
using AnnouncementNerdy.Domain.Entities.Announcement;


public record GetSimilarAnnouncementsQuery(string Id) : IRequest<CommonResult<IEnumerable<Announcement>>>;

public class GetSimilarAnnouncementsQueryHandler : IRequestHandler<GetSimilarAnnouncementsQuery, CommonResult<IEnumerable<Announcement>>>
{
    private readonly IAnnouncementRepository _announcementRepository;

    public GetSimilarAnnouncementsQueryHandler(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }
    
    public async Task<CommonResult<IEnumerable<Announcement>>> Handle(GetSimilarAnnouncementsQuery request, CancellationToken cancellationToken)
    {
        var announcement = await _announcementRepository.GetSimilar(request.Id);

        if (!announcement.Any())
        {
            return Failure<IEnumerable<Announcement>>("There is not simmilar announcements");
        }

        return Success(announcement);
    }
}

public class GetSimilarAnnouncementsQueryValidator : AbstractValidator<GetSimilarAnnouncementsQuery>
{
    public GetSimilarAnnouncementsQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}