using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Entity;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Features.AuthServices.Signup
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly TokenServices tokenServices;
        private readonly ILogger<SignUpHandler> logger;
        public SignUpHandler(IUserRepository userRepository, TokenServices tokenServices, ILogger<SignUpHandler> logger)
        {
            this.userRepository = userRepository;
            this.tokenServices = tokenServices;
            this.logger = logger;
        }

        public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Rozpoczęto proces rejestracji dla użytkownika: {Username}, Email: {Email}", request.username, request.email);
            try
            {
                bool exists = await userRepository.IsAnyUserWithUsernameAndEmailLikeThat(request.username, request.email);
                if (exists)
                {
                    logger.LogWarning("Próba rejestracji odrzucona: Login lub Email jest już zajęty ({Username})", request.username);
                    throw new Exception("User with that username or email already exists");
                }
                var user = User.Create(
                    new Username(request.username),
                    new Password(request.password),
                    new Fullname(request.name,
                    request.surname),
                    new Email(request.email)
                );
                user = await userRepository.CreateUserWithDefaultRole(user, cancellationToken);
                logger.LogInformation("Użytkownik {Username} został pomyślnie zarejestrowany z ID: {UserId}", user.Username.Value, user.Id);
                var accessToken = tokenServices.generateAccessToken(user.Id, user.Username.Value, user.UserRoles);
                var refreshToken = await tokenServices.GenerateRefreshToken(user.Id, user.Username.Value, cancellationToken);
                return new SignUpResponse(user.Username.Value, accessToken, refreshToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Błąd podczas rejestracji użytkownika {Username}: {ErrorMessage}", request.username, ex.Message);
                throw;
            }
        }
    }
}
