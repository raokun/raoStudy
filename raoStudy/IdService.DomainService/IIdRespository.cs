using IdService.DomainService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdService.DomainService
{
    public interface IIdRespository
    {
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByName(string name);
    }
}
