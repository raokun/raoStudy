
using SearchService.Domain.Objects;

namespace SearchService.Domain
{
    public interface ISearchRepository
    {
        #region  DemoObj
        public Task UpdateAsync(DemoObj obj);
        public Task DeleteAsync(DemoObj obj);

        public Task<DemoObjVo> SearchDemoObjs(string keyWord, int page, int pageSize);
        #endregion
    }
}
