using Microsoft.Extensions.Logging;

namespace AnnouncementNerdy.Application.Requests.Commands.Announcement;

using FluentValidation;
using Repositories;
using Domain.Results;
using AnnouncementNerdy.Domain.Entities.Announcement;


public record CreateAnnouncementCommand(string Title, string Description) : IRequest<CommonResult<string>>;

public class CreateAnnouncementCommandHandler : IRequestHandler<CreateAnnouncementCommand, CommonResult<string>>
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly ILogger<CreateAnnouncementCommandHandler> _logger;

    public CreateAnnouncementCommandHandler(IAnnouncementRepository announcementRepository, ILogger<CreateAnnouncementCommandHandler> logger)
    {
        _announcementRepository = announcementRepository;
        _logger = logger;
    }
    
    public async Task<CommonResult<string>> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = await _announcementRepository.AddAsync(new Announcement(request.Title, request.Description));

            return Success(id);
        }
        catch (Exception e)
        {
            _logger.LogError("Something went wrong in {@Handler}, Error:{@Error}", nameof(CreateAnnouncementCommandHandler), e.Message);
            throw;
        }
        
    }
}

public class CreateWalkerCommandValidator : AbstractValidator<CreateAnnouncementCommand>
{
    public CreateWalkerCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
    }
}