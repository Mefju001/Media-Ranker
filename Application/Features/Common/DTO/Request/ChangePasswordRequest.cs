namespace Application.Features.Common.DTO.Request
{
    public record ChangePasswordRequest(string newPassword, string confirmPassword, string oldPassword);
}
