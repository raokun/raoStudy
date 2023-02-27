using Microsoft.AspNetCore.Identity;
using Raok.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdService.DomainService.Entities
{
    public class User:IdentityUser<Guid>,ISoftDelete
    {
        public DateTime createTime { get; init; }
        public DateTime updateTime { get; set; }

        public bool IsDeleted { get; set; }

        public User(string userName) :base(userName){
            Id = Guid.NewGuid();
            createTime = DateTime.Now;
        }

        public void SoftDelete()
        {
            this.IsDeleted = true;
            this.updateTime = DateTime.Now;
        }
    }
}
