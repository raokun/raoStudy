using Microsoft.Extensions.DependencyInjection;
using Raok.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raok.JWT {
    public class ModuleInitializer : IModuleInitializer {
        public void Initialize(IServiceCollection services) {
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
