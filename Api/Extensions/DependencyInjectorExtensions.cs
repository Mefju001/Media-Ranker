using Application.Features.Common.Interfaces;
using System.Reflection;

namespace Api.Extensions
{
    public static class DependencyInjectorExtensions
    {
        public static void RegisterAllTypes(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes().Where(t =>
            t.IsClass &&
            !t.IsAbstract &&
            !t.IsInterface &&
            !t.IsGenericTypeDefinition);
            foreach (var type in types)
            {
                if (typeof(MediatR.IBaseRequest).IsAssignableFrom(type))
                    continue;
                if (type.Name.EndsWith("Response") || type.Name.EndsWith("Dto") || type.Name.EndsWith("Request")||type.Name.EndsWith("Notification"))
                    continue;
                var interfaces = type.GetInterfaces();
                if (typeof(BackgroundService).IsAssignableFrom(type)) continue;
                if (interfaces.Any())
                {
                    foreach (var @interface in interfaces)
                    {
                        if (@interface.Namespace != null && @interface.Namespace.StartsWith("System"))
                            continue;
                        services.AddScoped(@interface, type);
                    }
                    services.AddScoped(type);
                }
            }
        }
    }
}
