namespace AnnouncementNerdy.Application.Tests.Requests.Query.Announcement;


using AnnouncementNerdy.Application.Requests.Queries.Announcement;

using AnnouncementNerdy.Domain.Entities.Announcement;
using Repositories;
using TestUtils.Factory;


public class GetSimilarAnnouncementsQueryTests
{
    private readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
    
    public GetSimilarAnnouncementsQueryTests()
    {
        _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
    }

    [Fact]
    public async Task HandleGetSimilarAnnouncementsQuery_WhenAnnouncementDoesNotExist_ShouldReturnFailure()
    {
        //arrange
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetSimilar(It.IsAny<string>()))
            .ReturnsAsync(() => new List<Announcement>());

        var command = CreateAnnouncementQueryUtils.GetSimilarAnnouncementsQueryOrderAscending();

        var handler = new GetSimilarAnnouncementsQueryHandler(
            _announcementRepositoryMock.Object, new Logger<GetSimilarAnnouncementsQueryHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("There is not similar announcements");
    }
    
    
    [Fact]
    public async Task HandleGetSimilarAnnouncementsQuery_WhenCommandIsValidAndOrderedByAscending_ShouldReturnAnnouncementOrderedAscending()
    {
        //arrange
        var validAnnouncementList = Factory.AnnouncementFactory.CreateAnnouncementList();
        
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetSimilar(It.IsAny<string>()))
            .ReturnsAsync(() => validAnnouncementList);
        
        var command = CreateAnnouncementQueryUtils.GetSimilarAnnouncementsQueryOrderAscending();

        var handler = new GetSimilarAnnouncementsQueryHandler(
            _announcementRepositoryMock.Object, new Logger<GetSimilarAnnouncementsQueryHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeOfType<List<Announcement>>();
        result.Data.Should().BeEquivalentTo(validAnnouncementList.OrderBy(x => x.CreatedDate));
    }
    
    [Fact]
    public async Task HandleGetSimilarAnnouncementsQuery_WhenCommandIsValidAndOrderedByDescending_ShouldReturnAnnouncementOrderedDescending()
    {
        //arrange
        var validAnnouncementList = Factory.AnnouncementFactory.CreateAnnouncementList();
        
        _announcementRepositoryMock.Setup(
                x =>
                    x.GetSimilar(It.IsAny<string>()))
            .ReturnsAsync(() => validAnnouncementList);
        
        var command = CreateAnnouncementQueryUtils.GetSimilarAnnouncementsQueryOrderDescending();

        var handler = new GetSimilarAnnouncementsQueryHandler(
            _announcementRepositoryMock.Object, new Logger<GetSimilarAnnouncementsQueryHandler>(new LoggerFactory()));

        //act
        var result = await handler.Handle(command, default);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeOfType<List<Announcement>>();
        result.Data.Should().BeEquivalentTo(validAnnouncementList.OrderByDescending(x => x.CreatedDate));
    }
    
}