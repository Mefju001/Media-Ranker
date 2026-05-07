using Application.Features.Auth.RefreshAccessToken;
using Application.Features.Common.Interfaces;

namespace Application.Features.AuthServices.RefreshAccessToken
{
    public record RefreshAccessTokenCommand(string RefreshToken) : ICommand<TokenResponse?>;
}
