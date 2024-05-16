using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    // this is a extension class where we put some services that was originally located in program.cs
    // extension class need to be static, that means we can use the methods in this class without instantiate a new instance of this class
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // add DataContext as a service, so that we can inject into other parts of the application
            services. AddDbContext<DataContext>(opt => 
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        // TokenService is the implementation class of the interface "ITokenService", we need to provide it here as well
        // there are other options on how long the token will last
        // use services.AddTransient, the service will be disposed of within the request as soon it's been used and finished with
        // use services.AddScoped, the service will be disposed of after the controllers are disposed of at the end of the HTTP request
        // use services.AddSingleton, create a service that was instantiated when the application starts, and disposed of when the application was closed down. A good use case of this is caching service
        
        return services;
        }
    }
}