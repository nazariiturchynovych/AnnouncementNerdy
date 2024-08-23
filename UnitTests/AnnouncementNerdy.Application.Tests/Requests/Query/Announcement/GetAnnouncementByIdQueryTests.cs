using AnnouncementNerdy.Application.Requests.Queries.Announcement;

namespace AnnouncementNerdy.Application.Tests.Requests.Query.Announcement;

using Repositories;
using TestUtils.Factory;
using Domain.Errors;


public class GetAnnouncementByIdQueryTests
{
    private readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
    
    public GetAnnouncementByIdQueryTests()
    {
        _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
    }

    [Fact]
    public async Task HandleGetAnnouncementByIdQuery_WhenAnnouncementDoesNotExist_ShouldReturnFailure()
    {
        //arrange
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => default);

        var command = CreateAnnouncementQueryUtils.GetAnnouncementByIdQuery();

        var handler = new GetAnnouncementByIdQueryHandler(
            _announcementRepositoryMock.Object, new Logger<GetAnnouncementByIdQueryHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be(CommonErrors.EntityDoesNotExist);
    }
    
    
    [Fact]
    public async Task HandleGetAnnouncementByIdQuery_WhenCommandIsValid_ShouldReturnAnnouncement()
    {
        //arrange
        var validAnnouncement = Factory.AnnouncementFactory.CreateAnnouncement();
        
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => validAnnouncement);
        

        var command = CreateAnnouncementQueryUtils.GetAnnouncementByIdQuery();

        var handler = new GetAnnouncementByIdQueryHandler(
            _announcementRepositoryMock.Object, new Logger<GetAnnouncementByIdQueryHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(validAnnouncement);
    }
    
}