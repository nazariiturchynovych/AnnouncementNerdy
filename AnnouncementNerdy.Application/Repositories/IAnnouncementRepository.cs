using AnnouncementNerdy.Domain.Entities.Announcement;

namespace AnnouncementNerdy.Application.Repositories;

public interface IAnnouncementRepository
{
    Task<Announcement?> GetByIdAsync(string id);
    Task<IEnumerable<Announcement>> GetListAsync();
    Task<IEnumerable<Announcement>> GetSimilar(string id);
    Task<string> AddAsync(Announcement announcement);
    Task<bool> UpdateAsync(Announcement announcement);
    Task<bool> DeleteAsync(string id);
}