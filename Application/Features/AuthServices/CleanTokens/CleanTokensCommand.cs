using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.AuthServices.CleanTokens
{
    public record CleanTokensCommand : ICommand<Unit>
    {
    }
}
