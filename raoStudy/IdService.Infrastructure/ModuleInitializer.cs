using IdService.DomainService;
using Microsoft.Extensions.DependencyInjection;
using Raok.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IdService.Infrastructure
{
    class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IIdRespository,IdRepository>();
        }
    }
}
