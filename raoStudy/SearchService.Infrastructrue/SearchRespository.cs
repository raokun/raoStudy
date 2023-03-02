using Nest;
using Raok.Common;
using SearchService.Domain;
using SearchService.Domain.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Infrastructrue
{
    public class SearchRespository : ISearchRepository
    {
        private readonly IElasticClient elasticClient;

        private string keyStr = "DemoObj";

        public SearchRespository(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public Task DeleteAsync(DemoObj obj)
        {
            var res = elasticClient.DeleteByQuery<DemoObj>(m => m.Index(keyStr).Query(q => q.Term(f => f.ObjId, "elasticsearch.pm")));
            //因为有可能文档不存在，所以不检查结果
            //如果Episode被删除，则把对应的数据也从Elastic Search中删除
            return elasticClient.DeleteAsync(new DeleteRequest(keyStr, obj.ObjId));
        }

        public async Task<DemoObjVo> SearchDemoObjs(string keyWord, int page, int pageSize)
        {
            int form =pageSize*( page = 1);
            string kw = keyWord;
            Func<QueryContainerDescriptor<DemoObj>, QueryContainer> query = (q) =>
                q.Match(m => m.Field(f => f.ObjName).Query(kw)) || q.Match(m => m.Field(f => f.ObjRealName).Query(kw));
            Func<HighlightDescriptor<DemoObj>, IHighlight> highlightSelector = h => h.Fields(f => f.Field(fi => fi.ObjName));//高亮显示
            var result = await elasticClient.SearchAsync<DemoObj>(m => m.Index(keyStr).From(form).Size(pageSize).Query(query).Highlight(highlightSelector));
            if (!result.IsValid)
            {
                throw result.OriginalException;
            }
            List<DemoObj> demoObjs = new List<DemoObj>();
            foreach (var hit in result.Hits)
            {
                string highlightString;
                //如果没有预览内容，则显示前50个字
                if (hit.Highlight.ContainsKey("ObjName"))
                {
                    highlightString = string.Join("\r\n", hit.Highlight["ObjName"]);
                }
                else
                {
                    highlightString = hit.Source.ObjName.Cut(50);
                }
                var obj=hit.Source with { ObjName = highlightString };
                demoObjs.Add(obj);
            }
            return new DemoObjVo(demoObjs, result.Total);
        }

        public async Task UpdateAsync(DemoObj obj)
        {
            var res =await elasticClient.IndexAsync(obj, idx => idx.Index(keyStr).Id(obj.ObjId));
            if (!res.IsValid)
            {
                throw new ApplicationException(res.DebugInformation);
            }
        }
    }
}
