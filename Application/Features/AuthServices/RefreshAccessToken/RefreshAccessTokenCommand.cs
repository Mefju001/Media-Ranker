using Application.Common.DTO.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AuthServices.RefreshAccessToken
{
    public record RefreshAccessTokenCommand(string accessToken): IRequest<TokenResponse>;
}
