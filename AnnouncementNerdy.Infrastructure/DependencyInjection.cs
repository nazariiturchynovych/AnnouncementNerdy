using AnnouncementNerdy.Application.Repositories;
using AnnouncementNerdy.Domain.Entities.Announcement;
using AnnouncementNerdy.Infrastructure.Repositories;

namespace AnnouncementNerdy.Infrastructure;

using Microsoft.Extensions.Configuration;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddElasticSearch(configuration)
            .AddRepositories();
    }

    public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        var credentials = configuration.GetSection("Elastic");
        var settings = new ConnectionSettings(new Uri("https://localhost:9200"))
            .BasicAuthentication(credentials["user"], credentials["pass"])
            .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
            .DefaultIndex("announcement")
            .DefaultMappingFor<Announcement>(x => x.IndexName("announcement").IdProperty(x => x.Id));

        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
    }
}