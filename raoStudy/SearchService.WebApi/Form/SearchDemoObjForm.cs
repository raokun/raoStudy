using FluentValidation;

namespace SearchService.WebApi.Form
{
    public record SearchDemoObjForm(string Keyword, int PageIndex, int PageSize);
    public class SearchDemoObjFormValidator : AbstractValidator<SearchDemoObjForm>
    {
        public SearchDemoObjFormValidator()
        {
            RuleFor(e => e.Keyword).NotNull().MinimumLength(2).MaximumLength(100);
            RuleFor(e => e.PageIndex).GreaterThan(0);//页号从1开始
            RuleFor(e => e.PageSize).GreaterThanOrEqualTo(5);
        }
    }
}
