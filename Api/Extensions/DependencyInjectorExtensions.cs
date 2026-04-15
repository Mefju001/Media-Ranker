using System.Reflection;

namespace Api.Extensions
{
    public static class DependencyInjectorExtensions
    {
        public static void RegisterAllTypes(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface);
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                if (typeof(BackgroundService).IsAssignableFrom(type)) continue;
                if (type.Name.EndsWith("Service") || type.Name.EndsWith("Repository") || type.Name.EndsWith("Context"))
                {
                    foreach (var @interface in interfaces)
                    {
                        services.AddScoped(@interface, type);
                    }
                    services.AddScoped(type);
                }
            }
        }
    }
}
