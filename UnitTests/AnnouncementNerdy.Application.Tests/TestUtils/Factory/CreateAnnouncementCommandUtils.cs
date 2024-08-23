using AnnouncementNerdy.Application.Requests.Commands.Announcement;

namespace AnnouncementNerdy.Application.Tests.TestUtils.Factory;

public static class CreateAnnouncementCommandUtils
{
    public static CreateAnnouncementCommand CreateAnnouncementCommand()
    {
        return new CreateAnnouncementCommand(
            Title: Constants.Constants.ValidAnnouncement.Title,
            Description: Constants.Constants.ValidAnnouncement.Description
            );
    }

    public static DeleteAnnouncementCommand DeleteAnnouncementCommand()
    {
        return new DeleteAnnouncementCommand(
            Id: Constants.Constants.ValidAnnouncement.Id);
    }

    public static UpdateAnnouncementCommand UpdateAnnouncementCommand()
    {
        return new UpdateAnnouncementCommand(
            Id: Constants.Constants.ValidAnnouncement.Id,
            Title: Constants.Constants.ValidAnnouncement.Title,
            Description: Constants.Constants.ValidAnnouncement.Description
            );
    }
}