using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Entity;
using FluentValidation;
using Infrastructure;
using Infrastructure.BackgroundTasks;
using Infrastructure.BackgroundTasks.Interfaces;
using Infrastructure.BackgroundTasks.Workers;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Services;

namespace Api.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            services.AddSingleton<IBackgroundTaskQueue>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<BackgroundTaskQueue>>();
                return new BackgroundTaskQueue(logger);
            });


            services.AddScoped<IReferenceDataService, ReferenceDataService>();


            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddHostedService<QueueProcessorService>();
            services.AddHostedService<TokenBackgroundService>();
            services.AddScoped<AccessTokenService>();
            services.AddScoped<RefreshTokenService>();
            services.AddHttpClient<LogSenderService>();
            services.AddScoped<IPasswordHasher<UserDomain>, PasswordHasher<UserDomain>>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Application.Features.MovieServices.GetAll.GetAllQuery).Assembly);
            });
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
