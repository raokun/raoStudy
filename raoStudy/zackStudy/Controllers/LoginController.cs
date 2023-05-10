using IdService.DomainService;
using IdService.DomainService.Entities;
using IdService.Infrastructure;
using IdService.WebApi.Controllers;
using IdService.WebApi.Dto;
using IdService.WebApi.Form;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raok.Common;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace IdService.WebApi.Controllers {
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        
        private readonly ILogger<LoginController> _logger;
        private readonly IIdRespository _idRespository;
        private readonly IdDomainservice _idDomainservice;
        private readonly IConnectionMultiplexer redisConn;

        public LoginController(ILogger<LoginController> logger, IIdRespository idRespository, IdDomainservice idDomainservice, IConnectionMultiplexer connectionMultiplexer) {
            _logger = logger;
            _idRespository = idRespository;
            _idDomainservice = idDomainservice;
            this.redisConn = connectionMultiplexer;
        }

        [HttpGet]
        [Authorize]
        public async Task<User?> GetUserByName(string name)
        {
            return await _idRespository.GetUserByNameAsync(name);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(UserForm form) {
            (var result,var user,var password) = await _idRespository.AddAdminUserAsync(form.Name,form.PhoneNum);
            if (!result.Succeeded) {
                return BadRequest(result.Errors.ShowErrors());
            }
            _logger.LogInformation($"创建用户成功:  id:{user.Id},name:{user.UserName},phone:{user.PhoneNumber},password:{password}");
            return Ok(password);
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<UserVo> FindById(Guid id) {
            var user=await _idRespository.GetUserByIdAsync(id);
            if (user == null) {
                return null;
            }
            return UserVo.Create(user);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteById(Guid id) {
            var res= await _idRespository.RemoveUserAsync(id);
            if (!res.Succeeded) {
                return BadRequest(res.Errors.ShowErrors());
            }
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<string>> LoginByPhonePassWord(LoginForm form) {
            (var result, var token) =await _idDomainservice.LoginByPhoneAndPwdAsync(form.PhoneNum,form.PassWord);
            if (result.Succeeded) return token;
            else if (result.IsLockedOut)
                return StatusCode((int)HttpStatusCode.Locked, "用户被锁定");
            else {
                return BadRequest("登陆失败："+result.ToString());
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserVo>> GetUserInfo() {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _idRespository.GetUserByIdAsync(Guid.Parse(userId));
            if (user == null)//可能用户注销了
            {
                return NotFound();
            }
            //出于安全考虑，不要机密信息传递到客户端
            //除非确认没问题，否则尽量不要直接把实体类对象返回给前端
            return UserVo.Create(user);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<string> test() {
            var auth=Response.Headers.Authorization;
            var str = Response.Headers["Authenrization"];
            return auth.ToString();
        }

        [HttpGet]
        public async Task<bool> addCache(string str) {
           var db= redisConn.GetDatabase();
            return await db.StringSetAsync("test",str,TimeSpan.FromMinutes(30));
        }

        [HttpGet]
        public async Task<string> findCache(string key) {
            var db = redisConn.GetDatabase();
            return await db.StringGetAsync(key);
        }

        [HttpPost]
        public async Task<bool> addHashCache(HashDemoDto dto) {
            JsonSerializerOptions options = new() { WriteIndented = true };
            string json = JsonSerializer.Serialize(dto, options);
            var db = redisConn.GetDatabase();
            return await db.HashSetAsync("User", "raokun", json);
        }
        /// <summary>
        /// Redis StackExchange.Redis 文档  https://stackexchange.github.io/StackExchange.Redis/Transactions.html
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Dictionary<string,HashDemoDto?>> getHashAllByKey(string key) {
            var db = redisConn.GetDatabase();
            var res= await db.HashGetAllAsync(key);
            return res.ToDictionary(m => m.Name.ToString(), m => m.Value.ToString().ParseJson<HashDemoDto>(), StringComparer.Ordinal);
        }
    }
}