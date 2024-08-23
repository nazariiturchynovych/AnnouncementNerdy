using AnnouncementNerdy.Application.Tests.TestUtils.Factory;

namespace AnnouncementNerdy.Application.Tests.Requests.Commands.Announcement;

using Repositories;
using AnnouncementNerdy.Application.Requests.Commands.Announcement;
using AnnouncementNerdy.Application.Tests.TestUtils.Constants;
using AnnouncementNerdy.Domain.Entities.Announcement;

public class CreateAnnouncementCommandTests
{
    private readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
    
    public CreateAnnouncementCommandTests()
    {
        _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
    }

    [Fact]
    public async Task HandleCreateAnnouncementCommand_WhenCommandIsValid_ShouldReturnId()
    {
        //arrange
        _announcementRepositoryMock.Setup(
                x =>
                    x.AddAsync(It.IsAny<Announcement>()))
            .ReturnsAsync(() => Constants.ValidAnnouncement.Id);

        var command = CreateAnnouncementCommandUtils.CreateAnnouncementCommand();

        var handler = new CreateAnnouncementCommandHandler(
           _announcementRepositoryMock.Object, new Logger<CreateAnnouncementCommandHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeEmpty();
    }
    
}