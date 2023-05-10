using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raok.JWT {
    public static class SwaggerGenOptinsExtensions {
        /// <summary>
        /// 为Swagger增加Authentication报文头
        /// </summary>
        /// <param name="swaggerGen"></param>
        public static void AddAuthenticationHeader(this SwaggerGenOptions swaggerGen) {
            var scheme = new OpenApiSecurityScheme {
                Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Authorization"
            };
            swaggerGen.AddSecurityDefinition("Authenrization", scheme);
            var requirement = new OpenApiSecurityRequirement();
            requirement[scheme] = new List<string>();
            swaggerGen.AddSecurityRequirement(requirement);
            
        }
    }
}
