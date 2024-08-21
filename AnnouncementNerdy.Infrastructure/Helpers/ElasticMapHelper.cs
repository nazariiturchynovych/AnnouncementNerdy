using AnnouncementNerdy.Domain.Entities.Base;
using Nest;

namespace AnnouncementNerdy.Infrastructure.Helpers;

public class ElasticMapHelper<T> where T : class, IEntity<string>
{
    public static IEnumerable<T> MapElasticHitsToEntityWithIds(IReadOnlyCollection<IHit<T>> hits)
    {
        var entities = new List<T>();

        foreach (var hit in hits)
        {
            var entity = hit.Source;
            if (entity != null) 
            {
                entity.Id = hit.Id; 
                entities.Add(entity);
            }
        }
        
        return entities;
    }
}