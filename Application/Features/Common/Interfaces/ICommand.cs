using MediatR;

namespace Application.Features.Common.Interfaces
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
