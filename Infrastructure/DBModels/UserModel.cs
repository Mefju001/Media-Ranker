using Domain.Entity;
using Domain.Value_Object;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DBModels
{
    public class UserModel:IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActived { get; set; }
        public virtual ICollection<IdentityUserRole<Guid>> Roles { get; set; }
        public UserModel() { }
        public UserModel(Guid id, string username, string password, string name, string surname, string email, DateTime createdAt, bool isActived)
        {
            Id = id;
            UserName = username;
            PasswordHash = password;
            Name = name;
            Surname = surname;
            Email = email;
            CreatedAt = createdAt;
            IsActived = isActived;
        }
    }
}
