using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Raok.Common
{
    public static class ModuleInitializerExtensions
    {
        public static IServiceCollection RunModuleInitializerExtensions(this ServiceCollection services,IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types.Where(m=>!m.IsAbstract && typeof(IModuleInitializer).IsAssignableFrom(m)))
                {
                    var initializer=(IModuleInitializer?)Activator.CreateInstance(type);
                    if(initializer ==null)
                    {
                        throw new ApplicationException($"Cannot create ${type}");
                    }
                    initializer.Initialize(services);
                }
            }
            return services;
        }
    }
}
