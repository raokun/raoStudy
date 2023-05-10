using IdService.DomainService;
using IdService.DomainService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public readonly IdUserManager userManager;
        public readonly RoleManager<Role> roleManager;
        public readonly ILogger<IdRepository> logger;

        public IdRepository(IdUserManager userManager, RoleManager<Role> roleManager, ILogger<IdRepository> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }


        public async Task<(IdentityResult, User?, string? password)> AddAdminUserAsync(string userName, string phoneNum) {
            if (await GetUserByNameAsync(userName) != null) {
                return (IdentityError($"用户名已存在:{userName}"), null, null);
            }
            if (await GetUserByPhoneNumAsync(phoneNum) != null) {
                return (IdentityError($"该手机号码已注册过:{phoneNum}"), null, null);
            }
            User user = new User(userName);
            user.PhoneNumber = phoneNum;
            user.PhoneNumberConfirmed = true;
            string password = GeneratePassword();
            var result = await CreateAsync(user, password);
            if (!result.Succeeded) {
                return (result, null, null);
            }
            result =await AddToRoleAsync(user, "Admin");
            if (!result.Succeeded) {
                return (result, null, null);
            }
            return (IdentityResult.Success, user, password);
        }

        public async  Task<User?> GetUserByIdAsync(Guid id)
        {
            logger.LogInformation($"查询用户：{id}");
            return await userManager.FindByIdAsync(id.ToString());
        }

        public async  Task<User?> GetUserByNameAsync(string name)
        {
            return await userManager.FindByNameAsync(name);
        }

        public async  Task<User?> GetUserByPhoneNumAsync(string phoneNum) {
            return await userManager.Users.FirstOrDefaultAsync(m=>m.PhoneNumber==phoneNum);
        }

        private static IdentityResult IdentityError(string msg) {
            IdentityError error = new IdentityError { Description = msg };
            return IdentityResult.Failed(error);
        }

        private string GeneratePassword() {
            var options = userManager.Options.Password;
            int length = options.RequiredLength;
            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;
            StringBuilder password = new StringBuilder();
            Random random = new Random();
            while (password.Length < length) {
                char c = (char)random.Next(32, 126);
                password.Append(c);
                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));
            return password.ToString();
        }

        public Task<IdentityResult> CreateAsync(User user, string password) {
            return  userManager.CreateAsync(user,password);
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string roleName) {
            if (!await roleManager.RoleExistsAsync(roleName)) {
                Role role = new Role { Name=roleName};
                var result =await roleManager.CreateAsync(role);
                if (!result.Succeeded) {
                    return result;
                }
            }
            return await userManager.AddToRoleAsync(user,roleName);
        }

        public async Task<IdentityResult> RemoveUserAsync(Guid id) {
            var user = await GetUserByIdAsync(id);
            var userLoginStore = userManager.UserLoginStore;
            var nonCT = default(CancellationToken);
            var logins = await userLoginStore.GetLoginsAsync(user, nonCT);
            foreach (var log in logins) {
                await userLoginStore.RemoveLoginAsync(user, log.ProviderKey, log.LoginProvider, nonCT);
            }
            user.SoftDelete();
            return await userManager.UpdateAsync(user);
        }

        public Task<IList<string>> GetRolesAsync(User user) {
            return userManager.GetRolesAsync(user);
        }

        public async  Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure) {
            if (await userManager.IsLockedOutAsync(user)) {
                return SignInResult.LockedOut;
            }
            var success = await userManager.CheckPasswordAsync(user, password);
            if (success) {
                return SignInResult.Success;
            }
            else {
                if (lockoutOnFailure) {
                    var r = await AccessFailedAsync(user);
                    if (!r.Succeeded) {
                        throw new ApplicationException("AccessFailed failed");
                    }
                }
                return SignInResult.Failed;
            }
        }

        public Task<IdentityResult> AccessFailedAsync(User user) {
            return userManager.AccessFailedAsync(user);
        }
    }
}

