using Microsoft.AspNetCore.Identity;
using Raok.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdService.DomainService.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public bool IsAdmin { get; set; }
        public Role()
        {
            this.Id = Guid.NewGuid();
        }
    }

}
