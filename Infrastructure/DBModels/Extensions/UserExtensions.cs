using Domain.Aggregate;
using Domain.Enums;
using Domain.Value_Object;


namespace Infrastructure.DBModels.Extensions
{
    public static class UserExtensions
    {
        public static User ToDomain(this UserModel model, List<ERole> roles)
        {
            return User.Create(
                new Username(model.UserName!),
                new Password(model.PasswordHash!),
                new Fullname(model.Name, model.Surname),
                new Email(model.Email!),
                model.Id,
                model.IsActived,
                roles
                );
        }
        public static UserModel ToModel(this User user)
        {
            return new UserModel(
                user.Id,
                user.Username.Value,
                user.Password.HashValue,
                user.Fullname.Name,
                user.Fullname.Surname,
                user.Email.Value,
                user.AuditInfo.CreatedAt,
                user.AuditInfo.UpdatedAt,
                user.IsActive);
        }
    }
}
