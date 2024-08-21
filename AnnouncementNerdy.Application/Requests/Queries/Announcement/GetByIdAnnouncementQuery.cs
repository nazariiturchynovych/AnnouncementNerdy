using AnnouncementNerdy.Application.Repositories;
using AnnouncementNerdy.Domain.Results;
using FluentValidation;

namespace AnnouncementNerdy.Application.Requests.Queries.Announcement;

public record GetByIdAnnouncementQuery(string Id) : IRequest<CommonResult<AnnouncementNerdy.Domain.Entities.Announcement.Announcement>>;

public class GetByIdAnnouncementQueryHandler : IRequestHandler<GetByIdAnnouncementQuery, CommonResult>
{
    private readonly IAnnouncementRepository _announcementRepository;

    public GetByIdAnnouncementQueryHandler(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }
    
    public async Task<CommonResult> Handle(GetByIdAnnouncementQuery request, CancellationToken cancellationToken)
    {
        var announcement = await _announcementRepository.GetByIdAsync(request.Id);

        if (announcement is null)
        {
            return Failure("Entity does not exist");
        }

        return Success(announcement);
    }
}

public class GetByIdAnnouncementQueryValidator : AbstractValidator<GetByIdAnnouncementQuery>
{
    public GetByIdAnnouncementQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}