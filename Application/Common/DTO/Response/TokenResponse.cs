namespace Application.Common.DTO.Response
{
    public record TokenResponse(Guid userId, string username, string accessToken, string refreshToken);
}
