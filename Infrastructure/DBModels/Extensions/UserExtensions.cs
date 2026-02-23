using Domain.Entity;
using Domain.Value_Object;


namespace Infrastructure.DBModels.Extensions
{
    public static class UserExtensions
    {
        public static User ToDomain(this UserModel model,List<Role>roles)
        {
            return User.Reconstruct(
                model.Id,
                new Username(model.UserName),
                new Password(model.PasswordHash),
                new Fullname(model.Name, model.Surname),
                new Email(model.Email),
                new CreatedAt(model.CreatedAt),
                model.IsActived,
                roles
                );
        }
        public static UserModel ToModel(this User user)
        {
            return new UserModel(
                user.Id, 
                user.username.Value,
                user.password.HashValue,
                user.Fullname.Name,
                user.Fullname.Surname,
                user.Email.Value,
                user.CreatedAt.Value,
                user.IsActived);
        }
    }
}
