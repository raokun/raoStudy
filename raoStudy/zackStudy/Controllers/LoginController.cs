using IdService.DomainService;
using IdService.DomainService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace zackStudy.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        
        private readonly ILogger<LoginController> _logger;
        private readonly IIdRespository _idRespository;

        public LoginController(ILogger<LoginController> logger,IIdRespository idRespository)
        {
            _logger = logger;
            _idRespository = idRespository;
        }


        public async Task<User?> getUserByName(string name)
        {
            return await _idRespository.GetUserByName(name);
        }
    }
}