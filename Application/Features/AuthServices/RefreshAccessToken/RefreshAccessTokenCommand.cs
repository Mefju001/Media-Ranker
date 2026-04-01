using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.AuthServices.RefreshAccessToken
{
    public record RefreshAccessTokenCommand(string RefreshToken) : ICommand<TokenResponse>;
}
