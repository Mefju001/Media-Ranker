namespace Application.Features.User.ChangePassword
{
    public record ChangePasswordRequest(string newPassword, string confirmPassword, string oldPassword);
}
