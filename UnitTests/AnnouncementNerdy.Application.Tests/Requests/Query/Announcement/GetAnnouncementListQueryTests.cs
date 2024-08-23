namespace AnnouncementNerdy.Application.Tests.Requests.Query.Announcement;


using AnnouncementNerdy.Application.Requests.Queries.Announcement;

using AnnouncementNerdy.Domain.Entities.Announcement;
using Repositories;
using TestUtils.Factory;


public class GetAnnouncementListQueryTests
{
    private readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
    
    public GetAnnouncementListQueryTests()
    {
        _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
    }

    [Fact]
    public async Task HandleGetAnnouncementListQuery_WhenAnnouncementDoesNotExist_ShouldReturnFailure()
    {
        //arrange
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetListAsync())
            .ReturnsAsync(() => new List<Announcement>());

        var command = CreateAnnouncementQueryUtils.GetAnnouncementListQuery();

        var handler = new GetAnnouncementListQueryHandler(
            _announcementRepositoryMock.Object, new Logger<GetAnnouncementListQueryHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("There is no announcements");
    }
    
    
    [Fact]
    public async Task HandleGetAnnouncementListQuery_WhenCommandIsValid_ShouldReturnAnnouncement()
    {
        //arrange
        var validAnnouncementList = Factory.AnnouncementFactory.CreateAnnouncementList();
        
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetListAsync())
            .ReturnsAsync(() => validAnnouncementList);
        
        var command = CreateAnnouncementQueryUtils.GetAnnouncementListQuery();

        var handler = new GetAnnouncementListQueryHandler(
            _announcementRepositoryMock.Object, new Logger<GetAnnouncementListQueryHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeOfType<List<Announcement>>();
        result.Data.Should().BeEquivalentTo(validAnnouncementList);
    }
    
}