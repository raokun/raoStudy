using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Raok.JWT {
    public static class AuthenticationExtensions {
        public static AuthenticationBuilder AddJWTAuthentication(this IServiceCollection services,JWTOptions jWTOptions) {
            return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(m => {
                m.TokenValidationParameters = new() {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jWTOptions.Issuer,
                    ValidAudience = jWTOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTOptions.Key))
                };
            });
        }
    }
}