using AnnouncementNerdy.Application.Repositories;
using AnnouncementNerdy.Domain.Results;
using FluentValidation;

namespace AnnouncementNerdy.Application.Requests.Commands.Announcement;

public record UpdateAnnouncementCommand(string Id, string Title, string Description) : IRequest<CommonResult>;

public class UpdateAnnouncementCommandHandler : IRequestHandler<UpdateAnnouncementCommand, CommonResult>
{
    private readonly IAnnouncementRepository _announcementRepository;

    public UpdateAnnouncementCommandHandler(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }
    
    public async Task<CommonResult> Handle(UpdateAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var announcement = await _announcementRepository.GetByIdAsync(request.Id);

        if (announcement is null)
        {
            return Failure("Entity does not exist");
        }

        announcement.Title = request.Title;
        announcement.Description = request.Description;
        
        await _announcementRepository.UpdateAsync(announcement);

        return Success();
    }
}

public class UpdateWalkerCommandValidator : AbstractValidator<UpdateAnnouncementCommand>
{
    public UpdateWalkerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
    }
}