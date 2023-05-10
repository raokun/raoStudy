using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using Raok.Common;
using SearchService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Infrastructrue
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IElasticClient>(m=>
            {
                var options = m.GetRequiredService<IOptions<ElasticSearchOptions>>();
                var settings = new ConnectionSettings(options.Value.Url);
                return new ElasticClient(settings);
            });
            services.AddScoped<ISearchRepository, SearchRespository>();
        }
    }
}
