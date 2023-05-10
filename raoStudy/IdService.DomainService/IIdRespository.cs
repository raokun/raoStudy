using IdService.DomainService.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdService.DomainService
{
    public interface IIdRespository
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByNameAsync(string name);
        Task<User?> GetUserByPhoneNumAsync(string phoneNum);
        Task<IdentityResult> CreateAsync(User user, string password);//创建用户
        Task<(IdentityResult,User?,string? password)> AddAdminUserAsync(string userName, string phoneNum);
        Task<IdentityResult> AddToRoleAsync(User user,string roleName);
        Task<IdentityResult> AccessFailedAsync(User user);//记录一次登陆失败
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IdentityResult> RemoveUserAsync(Guid id);
        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IList<string>> GetRolesAsync(User user);
        /// <summary>
        /// 为了登录而检查用户名、密码是否正确
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="lockoutOnFailure">如果登录失败，则记录一次登陆失败</param>
        /// <returns></returns>
        public Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure);
    }
}
