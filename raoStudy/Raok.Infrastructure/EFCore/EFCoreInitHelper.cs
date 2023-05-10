using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace raok.Infrastructure {
    public static class EFCoreInitHelper
    {
        public static IServiceCollection AddAllDbContexts(this IServiceCollection services, Action<DbContextOptionsBuilder> builder, IEnumerable<Assembly> assemblies)
        {
            Type[] types = new Type[] { typeof(IServiceCollection), typeof(Action<DbContextOptionsBuilder>), typeof(ServiceLifetime), typeof(ServiceLifetime) };
            var methodAddDbContext = typeof(EntityFrameworkServiceCollectionExtensions).GetMethod(nameof(EntityFrameworkServiceCollectionExtensions.AddDbContext), 1, types);
            foreach (var assembly in assemblies)
            {
                Type[] typeAsm = assembly.GetTypes();
                foreach (var type in typeAsm.Where(m=>!m.IsAbstract && typeof(DbContext).IsAssignableFrom(m)))
                {
                    var methodGenericAddDbContext = methodAddDbContext.MakeGenericMethod(type);
                    methodGenericAddDbContext.Invoke(null, new object[] { services, builder, ServiceLifetime.Scoped, ServiceLifetime.Scoped });
                }
            }
            return services;
        }
    }
}