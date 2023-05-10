
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nacos.AspNetCore.V2;
using Nacos.V2.Naming.Dtos;
using raok.Infrastructure;
using Raok.Common;
using Raok.JWT;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Raok.CommonInitializer {
    public static class WebApplicationBuilderExtensions
    {

        public static void ConfigureDbConfiguration(this WebApplicationBuilder builder)
        {
            builder.Host.UseNacosConfig(section: "NacosConfig", parser: null, logAction: null);
        }

        public static void ConfigureExtraService(this WebApplicationBuilder builder)
        {
            Console.WriteLine("配置中");
            IServiceCollection service = builder.Services;
            IConfiguration configuration = builder.Configuration;
            service.AddNacosAspNet(builder.Configuration, section: "NacosConfig");
            var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
            service.RunModuleInitializerExtensions(assemblies);
            service.AddAllDbContexts(m =>
            {
                string connStr = configuration.GetValue<string>("ConnStr");
                Console.WriteLine($"connStr:{connStr}");
                //string connStr = "Data Source=localhost;port=3306;Initial Catalog=TNBLOG;uid=root;pwd=root;CharSet=utf8mb4;Allow User Variables=true;";
                m.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
            }, assemblies);
            //开始:Authentication,Authorization
            //只要需要校验Authentication报文头的地方（非IdentityService.WebAPI项目）也需要启用这些
            service.AddAuthorization();
            service.AddAuthentication();
            JWTOptions jwtOpt = configuration.GetSection("JWT").Get<JWTOptions>();
            builder.Services.AddJWTAuthentication(jwtOpt);
            builder.Services.Configure<SwaggerGenOptions>(m => {
                m.AddAuthenticationHeader();
            });
           
            //结束:Authentication,Authorization
            service.Configure<JWTOptions>(configuration.GetSection("JWT"));
            service.Configure<ForwardedHeadersOptions>(options => {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
            //Redis
            string redisConn = configuration.GetValue<string>("Redis:ConnStr");
            Console.WriteLine($"redisConn:{redisConn}");
            IConnectionMultiplexer redisConnMultiplexer = ConnectionMultiplexer.Connect(redisConn);
            service.AddSingleton(typeof(IConnectionMultiplexer),redisConnMultiplexer);


        }
    }
}
