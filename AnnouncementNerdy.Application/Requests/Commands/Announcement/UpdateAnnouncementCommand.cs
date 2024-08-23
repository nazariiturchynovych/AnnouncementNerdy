using AnnouncementNerdy.Application.Repositories;
using AnnouncementNerdy.Domain.Results;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnnouncementNerdy.Application.Requests.Commands.Announcement;

public record UpdateAnnouncementCommand(string Id, string Title, string Description) : IRequest<CommonResult>;

public class UpdateAnnouncementCommandHandler : IRequestHandler<UpdateAnnouncementCommand, CommonResult>
{
    private readonly IAnnouncementRepository _announcementRepository;
    private ILogger<UpdateAnnouncementCommandHandler> _logger;

    public UpdateAnnouncementCommandHandler(IAnnouncementRepository announcementRepository, ILogger<UpdateAnnouncementCommandHandler> logger)
    {
        _announcementRepository = announcementRepository;
        _logger = logger;
    }
    
    public async Task<CommonResult> Handle(UpdateAnnouncementCommand request, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError("Something went wrong in {@Command}, Error:{@Error}", nameof(UpdateAnnouncementCommand), e.Message);
            throw;
        }
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