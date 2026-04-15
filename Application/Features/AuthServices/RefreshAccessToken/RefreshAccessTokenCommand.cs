using Application.Common.DTO.Response;
using Application.Common.Interfaces;

namespace Application.Features.AuthServices.RefreshAccessToken
{
    public record RefreshAccessTokenCommand(string RefreshToken) : ICommand<TokenResponse>;
}
