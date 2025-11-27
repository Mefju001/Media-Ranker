using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO.Mapper;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class UserServices : IUserServices
    {
        private readonly IPasswordHasher<User> Hasher;
        private readonly IUnitOfWork unitOfWork;
        public UserServices(IPasswordHasher<User> passwordHasher,IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            Hasher = passwordHasher;
        }
        public async Task changePassword(string newPassword, string confirmPassword, string oldPassword, int userId)
        {
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentException("you should fill in these fields with passwords");
            }
            var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var passwordVerificationResult = user is null
               ? PasswordVerificationResult.Failed
               : Hasher.VerifyHashedPassword(user, user.password, oldPassword);
            if (!string.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                throw new PasswordMismatchException("The new password differed from the confirmation password");
            }
            if (passwordVerificationResult is not PasswordVerificationResult.Success)
                throw new InvalidCredentialsException("You write wrong old password");
            if (string.Equals(oldPassword, newPassword, StringComparison.Ordinal))
                throw new NewPasswordIsSameAsOldException("The new password is too similar to the old one");
                user.password = Hasher.HashPassword(user, newPassword);
                await unitOfWork.CompleteAsync();
        }
        public async Task changedetails(int userId, UserDetailsRequest userDetailsRequest)
        {
            if(userId<0) throw new ArgumentOutOfRangeException("userId");
            if(userDetailsRequest is null)
                throw new ArgumentException("you should fill that field");
            var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if(user is null) throw new UserNotFoundException("Not found user");
            var emailExist = await unitOfWork.Users.AnyAsync(u => u.email == userDetailsRequest.email && u.Id != user.Id);
            if (emailExist) throw new EmailAlreadyExistsException("This email is taken.");
            user.setUser(userDetailsRequest);
            await unitOfWork.CompleteAsync();
        }
        public async Task Delete(int id)
        {
            if(id < 0) throw new ArgumentOutOfRangeException("id");
            var user = await unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) throw new UserNotFoundException("Not found user with " + id);
            unitOfWork.Users.Delete(user);
            await unitOfWork.CompleteAsync();
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await unitOfWork.Users.AsQueryable()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.Reviews)
                .ThenInclude(r => r.Media)
                .ToListAsync();
            return users.Select(UserMapper.ToResponse).ToList();
        }
        public async Task<bool> Register(UserRequest userRequest)
        {
            bool exists = await unitOfWork.Users.AnyAsync(u => u.username == userRequest.username || u.email == userRequest.email);
            if (exists)
                return false;
            var user = new User
            {
                username = userRequest.username,
                password = Hasher.HashPassword(null, userRequest.password),
                name = userRequest.name,
                surname = userRequest.surname,
                email = userRequest.email,
            };
            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.CompleteAsync();
            var role = await unitOfWork.Roles.FirstOrDefaultAsync(r => r.role == ERole.User);
            if (role is null) return false;
            var UserRoles = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            await unitOfWork.UsersRoles.AddAsync(UserRoles);
            await unitOfWork.CompleteAsync();

            return true;
        }
        public async Task<UserResponse?> GetById(int id)
        {
            var user = await unitOfWork.Users.AsQueryable()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Reviews)
                    .ThenInclude(r => r.Media)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) return null;
            return UserMapper.ToResponse(user);
        }
        public async Task<List<UserResponse>> GetBy(string name)
        {
            var User = await unitOfWork.Users.AsQueryable()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Reviews)
                    .ThenInclude(r => r.Media)
                .Where(u =>
                        EF.Functions.Like(u.name, $"%{name}") ||
                        EF.Functions.Like(u.surname, $"{name}") ||
                        u.username == name ||
                        u.email == name)
                .ToListAsync();
            if (!User.Any()) return new List<UserResponse>();
            return User.Select(UserMapper.ToResponse).ToList();
        }
    }
}
