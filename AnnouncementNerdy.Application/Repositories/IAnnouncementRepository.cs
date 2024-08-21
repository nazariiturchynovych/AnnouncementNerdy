namespace AnnouncementNerdy.Application.Repositories;

using TestAssignment.Domain.Entities.Announcement;

public interface IAnnouncementRepository
{
    Task<Announcement?> GetByIdAsync(string id);
    Task<IEnumerable<Announcement>> GetListAsync();
    Task<IEnumerable<Announcement>> GetSimilar(string id);
    Task<string> AddAsync(Announcement announcement);
    Task<bool> UpdateAsync(Announcement announcement);
    Task<bool> DeleteAsync(string id);
}