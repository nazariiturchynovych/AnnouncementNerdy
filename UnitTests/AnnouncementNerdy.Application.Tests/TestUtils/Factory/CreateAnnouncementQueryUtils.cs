using AnnouncementNerdy.Application.Requests.Commands.Announcement;
using AnnouncementNerdy.Application.Requests.Queries.Announcement;
using AnnouncementNerdy.Domain.Enums;

namespace AnnouncementNerdy.Application.Tests.TestUtils.Factory;

public static class CreateAnnouncementQueryUtils
{
    public static GetAnnouncementByIdQuery GetAnnouncementByIdQuery()
    {
        return new GetAnnouncementByIdQuery(
            Id: Constants.Constants.ValidAnnouncement.Id
            );
    }

    public static GetAnnouncementListQuery GetAnnouncementListQuery()
    {
        return new GetAnnouncementListQuery();
    }

    public static GetSimilarAnnouncementsQuery GetSimilarAnnouncementsQueryOrderAscending()
    {
        return new GetSimilarAnnouncementsQuery(
            Id: Constants.Constants.ValidAnnouncement.Id, OrderBy.Ascending);
    }
    
    public static GetSimilarAnnouncementsQuery GetSimilarAnnouncementsQueryOrderDescending()
    {
        return new GetSimilarAnnouncementsQuery(
            Id: Constants.Constants.ValidAnnouncement.Id, OrderBy.Descending);
    }
}