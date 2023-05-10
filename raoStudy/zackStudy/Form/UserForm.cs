using FluentValidation;

namespace IdService.WebApi.Form {
    public record UserForm (string Name,string PhoneNum);
    public class UserFormValidator : AbstractValidator<UserForm> {
        public UserFormValidator() {
            RuleFor(m=>m.Name).NotNull().NotEmpty().MaximumLength(11).MinimumLength(2);
            RuleFor(m=>m.PhoneNum).NotNull().NotEmpty().MaximumLength(11);
        }
    }
}
