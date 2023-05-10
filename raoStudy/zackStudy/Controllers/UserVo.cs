using IdService.DomainService.Entities;

namespace IdService.WebApi.Controllers {
    public record UserVo (Guid id,string name,string phone,DateTime createTime) {
        public static UserVo Create(User user) {
            return new UserVo(user.Id,user.UserName,user.PhoneNumber,user.createTime);
        }
    }
}
