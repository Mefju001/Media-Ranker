using Application.Features.Common.Interfaces;
using Application.Features.Common.Notification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Application.Behaviours
{
    internal class SendNotificationBehaviour<TRequest, TResponse>(IServiceProvider serviceProvider) : IPipelineBehavior<TRequest, TResponse>
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
                    ? $"Zakończono: {requestName} (ID: {idValue}) w {timer.ElapsedMilliseconds}ms"
                    : $"Zakończono: {requestName} w {timer.ElapsedMilliseconds}ms";

                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = serviceProvider.CreateScope();
                        var scopedMediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await scopedMediator.Publish(new LogNotification("Information", message, requestName), CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Pipeline Logger Error]: {ex.Message}");
                    }
                }, CancellationToken.None);

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
