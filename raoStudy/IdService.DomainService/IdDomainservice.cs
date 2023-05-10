using IdService.DomainService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Raok.JWT;
using System.Security.Claims;

namespace IdService.DomainService
{
    public class IdDomainservice
    {
        private readonly IIdRespository respository;
        private readonly ITokenService tokenService;
        private readonly IOptions<JWTOptions> optJWT;

        public IdDomainservice(IIdRespository respository, ITokenService tokenService, IOptions<JWTOptions> optJWT) {
            this.respository = respository;
            this.tokenService = tokenService;
            this.optJWT = optJWT;
        }

        private async Task<string> BuildTokenAsync(User user) {
            var roles=await respository.GetRolesAsync(user);
            List<Claim>claims=new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()));
            foreach (var role in roles) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return tokenService.BuildToken(claims, optJWT.Value);
        }

        private async Task<SignInResult> CheckUserNameAndPwdAsync(string userName, string password) {
            var user = await respository.GetUserByNameAsync(userName);
            if (user == null) {
                return SignInResult.Failed;
            }
            //CheckPasswordSignInAsync会对于多次重复失败进行账号禁用
            var result = await respository.CheckForSignInAsync(user, password, true);
            return result;
        }

        private async Task<SignInResult> checkPhoneNumPwdAsync(string phoneNum, string passWord) {
            var user = await respository.GetUserByPhoneNumAsync(phoneNum);
            if (user == null) { return SignInResult.Failed; }
            var res = await respository.CheckForSignInAsync(user, passWord, true);
            return res;
        }

        public async Task<(SignInResult result,string? token)> LoginByPhoneAndPwdAsync(string phoneNum,string passWord) {
            var checkRes=await checkPhoneNumPwdAsync(phoneNum, passWord);
            if (!checkRes.Succeeded) {
                return (checkRes, null);
            }
            var user= await respository.GetUserByPhoneNumAsync(phoneNum);
            string token=await BuildTokenAsync(user);
            return (SignInResult.Success,token);
        }

    }
}