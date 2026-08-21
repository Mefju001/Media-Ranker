using Application.Features.Common.Interfaces;

namespace Application.Features.Auth.RefreshAccessToken
{
    public record RefreshAccessTokenCommand(string RefreshToken) : ICommand<TokenResponse?>;
}
