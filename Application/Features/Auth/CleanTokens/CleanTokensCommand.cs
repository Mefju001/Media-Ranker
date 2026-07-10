using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.Auth.CleanTokens
{
    public record CleanTokensCommand : ICommand<Unit>;
}
