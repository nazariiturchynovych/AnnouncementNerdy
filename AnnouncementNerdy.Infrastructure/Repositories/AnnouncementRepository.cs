using AnnouncementNerdy.Application.Repositories;
using AnnouncementNerdy.Domain.Entities.Announcement;
using AnnouncementNerdy.Infrastructure.Helpers;
using Nest;

namespace AnnouncementNerdy.Infrastructure.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly IElasticClient _elasticClient;

    public AnnouncementRepository(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<Announcement?> GetByIdAsync(string id) 
    {
        var response = (await _elasticClient.GetAsync<Announcement>(id));

        var announcement = response.Source;
        announcement.Id = response.Id;

        return announcement;
    }

    public async Task<IEnumerable<Announcement>> GetListAsync()
    {
        var result = await _elasticClient.SearchAsync<Announcement>(s => s
                .Query(q => q.MatchAll())
                .Size(10000)
                .Sort(x => x.Ascending(x =>
                    x.CreatedDate))
        );

        return ElasticMapHelper<Announcement>.MapElasticHitsToEntityWithIds(result.Hits);
    }

    public async Task<IEnumerable<Announcement>> GetSimilar(string id)
    {
        var announcemcent = (await _elasticClient.GetAsync<Announcement>(id)).Source;


        Func<QueryContainerDescriptor<Announcement>, QueryContainer> query = q =>
            q.Match(x => x.Field(f => f.Title).Query(announcemcent.Title)) &&
            q.Match(x => x.Field(f => f.Description).Query(announcemcent.Description));


        var result = await _elasticClient.SearchAsync<Announcement>(s => s.Query(query));


        return ElasticMapHelper<Announcement>.MapElasticHitsToEntityWithIds(result.Hits);
    }


    public async Task<string> AddAsync(Announcement announcement)
    {
        var indexName = nameof(Announcement).ToLower() + "s";
        var indexResponse = await _elasticClient.Indices.ExistsAsync(indexName);

        if (!indexResponse.Exists)
            await _elasticClient.Indices.CreateAsync(indexName, i => i.Map<Announcement>(x => x.AutoMap()));

        var response = await _elasticClient.IndexAsync<Announcement>(announcement, i => i.Index(indexName));
        return response.Id;
    }

    public async Task<bool> UpdateAsync(Announcement announcement)
    {
        var response = await _elasticClient.UpdateAsync<Announcement>(announcement.Id, u => u.Doc(announcement));
        return response.IsValid;
    }
    
    public async Task<bool> DeleteAsync(string id) => (await _elasticClient.DeleteAsync<Announcement>(id)).IsValid;
}