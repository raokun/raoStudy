using IdService.DomainService;
using IdService.DomainService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdService.Infrastructure
{
    public class IdRepository : IIdRespository
    {
        public readonly UserManager<User> userManager;
        public readonly RoleManager<Role> roleManager;
        public readonly ILogger<IdRepository> logger;

        public IdRepository(UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<IdRepository> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public Task<User?> GetUserById(Guid id)
        {
            logger.LogInformation($"查询用户：{id}");
            return userManager.FindByIdAsync(id.ToString());
        }

        public Task<User?> GetUserByName(string name)
        {
            return userManager.FindByNameAsync(name);
        }
    }
}
