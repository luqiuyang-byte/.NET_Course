using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // this service outlines how authentication works
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])), // we are getting TokenKey from configuration, so make sure it's spelled correctly otherwise it's not gonna work and no error shown
                    ValidateIssuer = false, // all token issued by API server, so no need to verify this
                    ValidateAudience = false
                };
            }); // in order for this service to be used, we need to add the middleware to authenticate the request
            // the middleware need to be added before the MapController method and after UseCors, in program.cs

            return services;
        }
    }
}