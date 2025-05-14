using UsersAdministration.Database.InMemoryDb;
using UsersAdministration.Services.Users;

namespace UsersAdministration.Extensions;

public static class ConfigurationExtension
{
    public static IServiceCollection AddMyServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IInMemRepository, InMemRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddControllers();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        return services;
    }
}