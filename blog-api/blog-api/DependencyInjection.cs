using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace blog_api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ISeedService, SeedService>();
        services.AddScoped<IJsonPlaceholderService, JsonPlaceHolderService>();
        services.AddScoped<IHttpClientHelper, HttpClientHelper>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddDbContext<IDataContext, DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("default"));
        });

        return services;
    }
}
