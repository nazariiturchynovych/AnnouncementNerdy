namespace AnnouncementNerdy.Infrastructure;


using Microsoft.Extensions.Configuration;
using TestAssignment.Domain.Entities.Announcement;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;


public static class DependencyInjection
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var credentials = configuration.GetSection("Elastic");
        var settings = new ConnectionSettings(new Uri("https://localhost:9200"))
            .BasicAuthentication(credentials["user"], credentials["pass"])
            .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
            .DefaultIndex("product")
            .DefaultMappingFor<Announcement>(x => x.IndexName("product"));
            
        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);
    }
}