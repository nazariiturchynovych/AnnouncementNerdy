namespace AnnouncementNerdy.Application.Requests.Commands.Announcement;

using FluentValidation;
using Repositories;
using Domain.Results;
using AnnouncementNerdy.Domain.Entities.Announcement;


public record CreateAnnouncementCommand(string Title, string Description) : IRequest<CommonResult<string>>;

public class CreateAnnouncementCommandHandler : IRequestHandler<CreateAnnouncementCommand, CommonResult<string>>
{
    private readonly IAnnouncementRepository _announcementRepository;

    public CreateAnnouncementCommandHandler(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }
    
    public async Task<CommonResult<string>> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
    {
       var id = await _announcementRepository.AddAsync(new Announcement(request.Title, request.Description));

        return Success(id);
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