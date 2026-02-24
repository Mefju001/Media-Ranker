using Domain.Entity;
using Domain.Enums;
using Domain.Value_Object;


namespace Infrastructure.DBModels.Extensions
{
    public static class UserExtensions
    {
        public static User ToDomain(this UserModel model,List<ERole>roles)
        {
            return User.Reconstruct(
                model.Id,
                new Username(model.UserName!),
                new Password(model.PasswordHash!),
                new Fullname(model.Name, model.Surname),
                new Email(model.Email!),
                new CreatedAt(model.CreatedAt),
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
                user.CreatedAt.Value,
                user.IsActive);
        }
    }
}
