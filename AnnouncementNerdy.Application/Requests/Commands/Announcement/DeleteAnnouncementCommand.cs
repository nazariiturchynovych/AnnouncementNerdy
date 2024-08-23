using AnnouncementNerdy.Application.Repositories;
using AnnouncementNerdy.Domain.Results;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnnouncementNerdy.Application.Requests.Commands.Announcement;

public record DeleteAnnouncementCommand(string Id) : IRequest<CommonResult>;

public class DeleteAnnouncementCommandHandler : IRequestHandler<DeleteAnnouncementCommand, CommonResult>
{
    private readonly IAnnouncementRepository _announcementRepository;
    private ILogger<DeleteAnnouncementCommandHandler> _logger;

    public DeleteAnnouncementCommandHandler(IAnnouncementRepository announcementRepository,
        ILogger<DeleteAnnouncementCommandHandler> logger)
    {
        _announcementRepository = announcementRepository;
        _logger = logger;
    }

    public async Task<CommonResult> Handle(DeleteAnnouncementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var announcement = await _announcementRepository.GetByIdAsync(request.Id);

            if (announcement is null)
            {
                return Failure("Entity does not exist");
            }

            var result = await _announcementRepository.DeleteAsync(announcement.Id);

            return Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong in {@Handler}, Error:{@Error}",
                nameof(DeleteAnnouncementCommandHandler), e.Message);
            throw;
        }
    }
}

public class DeleteWalkerCommandValidator : AbstractValidator<DeleteAnnouncementCommand>
{
    public DeleteWalkerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}