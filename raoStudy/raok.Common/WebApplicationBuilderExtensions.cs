using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nacos.AspNetCore.V2;
using raok.Common;

namespace Raok.Common
{
    public static class WebApplicationBuilderExtensions
    {

        public static void ConfigureDbConfiguration(this WebApplicationBuilder builder)
        {
            builder.Host.UseNacosConfig(section: "NacosConfig", parser: null, logAction: null);
        }

        public static void ConfigureExtraService(this WebApplicationBuilder builder)
        {
            IServiceCollection service = builder.Services;
            IConfiguration configuration = builder.Configuration;
            service.AddNacosAspNet(builder.Configuration, section: "NacosConfig");
            service.AddAuthorization();
            service.AddAuthentication();
            var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
            service.AddAllDbContexts(m =>
            {
                string connStr = configuration.GetValue<string>("DbConfig.ConnectionString");
                m.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
            }, assemblies);
        }
    }
}
