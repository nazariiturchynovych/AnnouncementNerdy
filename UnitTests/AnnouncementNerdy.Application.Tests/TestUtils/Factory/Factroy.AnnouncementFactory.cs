namespace AnnouncementNerdy.Application.Tests.TestUtils.Factory;


using Domain.Entities.Announcement;

public static partial class Factory
{
    public static class AnnouncementFactory
    {
        public static Announcement CreateAnnouncement()
        {
            var title = Constants.Constants.ValidAnnouncement.Title;
            var description = Constants.Constants.ValidAnnouncement.Description;

            return new Announcement(title, description);
        }
        
        public static List<Announcement> CreateAnnouncementList()
        {
            var title = Constants.Constants.ValidAnnouncement.Title;
            var description = Constants.Constants.ValidAnnouncement.Description;

            var list = new List<Announcement>();
            list.Add(new Announcement(title, description));
            list.Add(new Announcement(title, description));
            
            return list;
        }

        
    }
}