namespace UsersAdministration.Extensions;

public static class ConfigurationExtension
{
    public static IServiceCollection AddMyServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}