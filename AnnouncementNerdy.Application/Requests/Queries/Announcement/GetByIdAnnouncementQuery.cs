namespace AnnouncementNerdy.Application.Requests.Queries.Announcement;

using Repositories;
using Domain.Results;
using FluentValidation;
using AnnouncementNerdy.Domain.Entities.Announcement;

public record GetByIdAnnouncementQuery(string Id) : IRequest<CommonResult<Announcement>>;

public class GetByIdAnnouncementQueryHandler : IRequestHandler<GetByIdAnnouncementQuery, CommonResult<Announcement>>
{
    private readonly IAnnouncementRepository _announcementRepository;

    public GetByIdAnnouncementQueryHandler(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }
    
    public async Task<CommonResult<Announcement>> Handle(GetByIdAnnouncementQuery request, CancellationToken cancellationToken)
    {
        var announcement = await _announcementRepository.GetByIdAsync(request.Id);

        if (announcement is null)
        {
            return Failure<Announcement>("Entity does not exist");
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