using Application.Common.Interfaces;
using Application.Features.Auth.Common;
using Application.Features.Genres.GenreManager;
using Application.Features.Genres.GetAll;
using FluentValidation;
using Infrastructure.BackgroundTasks.CleanTokens.Workers;
using Infrastructure.BackgroundTasks.LogSender;
using Infrastructure.Database;
using Infrastructure.Database.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            services.Configure<JwtSettings>(config.GetSection(JwtSettings.SectionName));
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IMediaRepository<>), typeof(MediaRepository<>));
            services.RegisterAllTypes(typeof(GenreRepository).Assembly);
            services.RegisterAllTypes(typeof(IGenreManager).Assembly);
            services.AddHostedService<TokenBackgroundService>();
            services.AddHttpClient<LogSenderService>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(GetAllQuery).Assembly);
                cfg.AddOpenBehavior(typeof(ErrorHandlingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            });
            return services;
        }
    }
}
