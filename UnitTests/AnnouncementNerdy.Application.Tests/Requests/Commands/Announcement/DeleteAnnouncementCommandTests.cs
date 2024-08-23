namespace AnnouncementNerdy.Application.Tests.Requests.Commands.Announcement;

using Repositories;
using AnnouncementNerdy.Application.Requests.Commands.Announcement;
using AnnouncementNerdy.Application.Tests.TestUtils.Factory;
using Domain.Errors;


public class DeleteAnnouncementCommandTests
{
    private readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
    
    public DeleteAnnouncementCommandTests()
    {
        _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
    }

    [Fact]
    public async Task HandleDeleteAnnouncementCommand_WhenAnnouncementDoesNotExist_ShouldReturnFailure()
    {
        //arrange
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => default);

        var command = CreateAnnouncementCommandUtils.DeleteAnnouncementCommand();

        var handler = new DeleteAnnouncementCommandHandler(
            _announcementRepositoryMock.Object, new Logger<DeleteAnnouncementCommandHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be(CommonErrors.EntityDoesNotExist);
    }
    
    
    [Fact]
    public async Task HandleDeleteAnnouncementCommand_WhenCommandIsValid_ShouldReturnTrue()
    {
        //arrange
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => Factory.AnnouncementFactory.CreateAnnouncement());
        
        _announcementRepositoryMock.Setup(
                x =>
                    x.DeleteAsync(It.IsAny<string>()))
            .ReturnsAsync(() => true);

        var command = CreateAnnouncementCommandUtils.DeleteAnnouncementCommand();

        var handler = new DeleteAnnouncementCommandHandler(
           _announcementRepositoryMock.Object, new Logger<DeleteAnnouncementCommandHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeTrue();
    }
    
}