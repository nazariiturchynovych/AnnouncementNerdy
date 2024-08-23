namespace AnnouncementNerdy.Application.Tests.Requests.Commands.Announcement;

using Repositories;
using AnnouncementNerdy.Application.Requests.Commands.Announcement;
using AnnouncementNerdy.Application.Tests.TestUtils.Factory;
using Domain.Errors;
using AnnouncementNerdy.Domain.Entities.Announcement;

public class UpdateAnnouncementCommandTests
{
    private readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
    
    public UpdateAnnouncementCommandTests()
    {
        _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
    }

    [Fact]
    public async Task HandleCreateAnnouncementCommand_WhenAnouncementDoesNotExist_ShouldReturnFailure()
    {
        //arrange
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => default);

        var command = CreateAnnouncementCommandUtils.UpdateAnnouncementCommand();

        var handler = new UpdateAnnouncementCommandHandler(
            _announcementRepositoryMock.Object, new Logger<UpdateAnnouncementCommandHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be(CommonErrors.EntityDoesNotExist);
    }
    
    
    [Fact]
    public async Task HandleCreateAnnouncementCommand_WhenCommandIsValid_ShouldReturnTrue()
    {
        //arrange
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => Factory.AnnouncementFactory.CreateAnnouncement());
        
        _announcementRepositoryMock.Setup(
                x =>
                    x.UpdateAsync(It.IsAny<Announcement>()))
            .ReturnsAsync(() => true);

        var command = CreateAnnouncementCommandUtils.UpdateAnnouncementCommand();

        var handler = new UpdateAnnouncementCommandHandler(
           _announcementRepositoryMock.Object, new Logger<UpdateAnnouncementCommandHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeTrue();
    }
    
}