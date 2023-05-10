using FluentValidation;

namespace IdService.WebApi.Controllers {
    public record LoginForm (string PhoneNum,string PassWord);
    public class LoginFormValidator : AbstractValidator<LoginForm> {
        public LoginFormValidator() {
            RuleFor(x => x.PhoneNum).NotNull().NotEmpty();
            RuleFor(x => x.PassWord).NotNull().NotEmpty();
        }
    }
}
