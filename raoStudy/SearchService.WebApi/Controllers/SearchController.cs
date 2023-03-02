using Microsoft.AspNetCore.Mvc;
using SearchService.Domain;
using SearchService.Domain.Objects;
using SearchService.WebApi.Form;

namespace SearchService.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchRepository searchRepository;

        public SearchController(ISearchRepository searchRepository)
        {
            this.searchRepository = searchRepository;
        }
        [HttpGet]
        public  Task<DemoObjVo> SearchDemoObjs([FromQuery] SearchDemoObjForm form)
        {
            return  searchRepository.SearchDemoObjs(form.Keyword,form.PageIndex,form.PageSize);
        }


    }
}