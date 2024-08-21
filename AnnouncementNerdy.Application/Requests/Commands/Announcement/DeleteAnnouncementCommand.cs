using AnnouncementNerdy.Application.Repositories;
using AnnouncementNerdy.Domain.Results;
using FluentValidation;

namespace AnnouncementNerdy.Application.Requests.Commands.Announcement;

public record DeleteAnnouncementCommand(string Id) : IRequest<CommonResult>;

public class DeleteAnnouncementCommandHandler : IRequestHandler<DeleteAnnouncementCommand, CommonResult>
{
    private readonly IAnnouncementRepository _announcementRepository;

    public DeleteAnnouncementCommandHandler(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }
    
    public async Task<CommonResult> Handle(DeleteAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var announcement = await _announcementRepository.GetByIdAsync(request.Id);

        if (announcement is null)
        {
            return Failure("Entity does not exist");
        }
        
        await _announcementRepository.DeleteAsync(announcement.Id);

        return Success();
    }
}

public class DeleteWalkerCommandValidator : AbstractValidator<DeleteAnnouncementCommand>
{
    public DeleteWalkerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}