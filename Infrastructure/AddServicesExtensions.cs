using Application.Behaviours;
using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Infrastructure.BackgroundTasks.CleanTokens.Workers;
using Infrastructure.BackgroundTasks.LogSender;
using Infrastructure.Database;
using Infrastructure.Database.DBModels;
using Infrastructure.Database.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    internal static class AddServicesExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddHostedService<TokenBackgroundService>();
            services.AddHttpClient<LogSenderService>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IAppDbContext), typeof(AppDbContext));
            services.AddScoped(typeof(IMediaRepository<>), typeof(MediaRepository<>));
            services.RegisterAllTypes(typeof(DirectorRepository).Assembly);
            services.AddIdentity<UserModel, RoleModel>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
        }
    }
}
