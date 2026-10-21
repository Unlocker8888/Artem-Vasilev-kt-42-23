using ArtemVasilevKt_42_23.Interfaces;

namespace ArtemVasilevKt_42_23.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        return services;
    }
}