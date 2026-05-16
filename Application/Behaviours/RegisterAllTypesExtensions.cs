using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Application.Behaviours
{
    public static class RegisterAllTypesExtensions
    {
        public static void RegisterAllTypes(this IServiceCollection services, params Assembly[]assemblies)
        {
            services.Scan(s=>s
                    .FromAssemblies(assemblies)
                    .AddClasses(classes => classes.Where(t =>
                    !t.Name.EndsWith("Request") &&
                    !t.Name.EndsWith("Response")&&
                    !t.Name.EndsWith("Command")&&
                    !t.Name.EndsWith("Notification")&&
                    !t.Name.EndsWith("Dto")&&
                    !t.Name.EndsWith("Query")&&
                    
                    !typeof(IHostedService).IsAssignableFrom(t)&&

                    !t.Name.EndsWith("Behaviour") &&
                    !t.Name.EndsWith("Handler"))
                    ,publicOnly:false)
                    .AsMatchingInterface()
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
        }
    }
}
