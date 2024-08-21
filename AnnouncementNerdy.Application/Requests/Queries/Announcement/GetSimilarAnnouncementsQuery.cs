using AnnouncementNerdy.Application.Repositories;
using AnnouncementNerdy.Domain.Results;
using FluentValidation;

namespace AnnouncementNerdy.Application.Requests.Queries.Announcement;

public record GetSimilarAnnouncementsQuery(string Id) : IRequest<CommonResult<AnnouncementNerdy.Domain.Entities.Announcement.Announcement>>;

public class GetSimilarAnnouncementsQueryHandler : IRequestHandler<GetSimilarAnnouncementsQuery, CommonResult>
{
    private readonly IAnnouncementRepository _announcementRepository;

    public GetSimilarAnnouncementsQueryHandler(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }
    
    public async Task<CommonResult> Handle(GetSimilarAnnouncementsQuery request, CancellationToken cancellationToken)
    {
        var announcement = await _announcementRepository.GetSimilar(request.Id);

        if (!announcement.Any())
        {
            return Failure("There is not simmilar announcements");
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