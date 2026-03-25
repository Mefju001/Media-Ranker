using Application.Common.DTO.Response;
using MediatR;

namespace Application.Features.AuthServices.RefreshAccessToken
{
    public record RefreshAccessTokenCommand(string RefreshToken) : IRequest<TokenResponse>;
}
