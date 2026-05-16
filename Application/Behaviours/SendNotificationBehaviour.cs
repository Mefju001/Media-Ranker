using Application.Features.Common.Interfaces;
using Application.Features.Common.Notification;
using MediatR;
using System.Diagnostics;

namespace Application.Behaviours
{
    internal class SendNotificationBehaviour<TRequest, TResponse>(IMediator mediator) : IPipelineBehavior<TRequest, TResponse>
            where TRequest : ISendNotificationCommand
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var timer = Stopwatch.StartNew();
            try
            {
                var response = await next();
                timer.Stop();
                var idProperty = request.GetType().GetProperty("Id") ?? request.GetType().GetProperty("id");
                var idValue = idProperty?.GetValue(request, null);

                var message = idValue != null
                    ? $"Zakończono: {requestName} (ID: {idValue})"
                    : $"Zakończono: {requestName}";
                await mediator.Publish(new LogNotification("Information", message, requestName));
                return response;
            }
            catch (Exception)
            {
                timer.Stop();
                throw;
            }
        }
    }
}
