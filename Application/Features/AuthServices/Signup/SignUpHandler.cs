using Application.Common.DTO;
using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Aggregate;
using Domain.DomainService;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Features.AuthServices.Signup
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly ITokenService tokenService;
        private readonly ILogger<SignUpHandler> logger;
        public SignUpHandler(IUserRepository userRepository, IUserDetailsRepository userDetailsRepository, ITokenService tokenService, ILogger<SignUpHandler> logger)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.logger = logger;
            this.userDetailsRepository = userDetailsRepository;
        }

        public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            bool exists = await userRepository.IsAnyUserWithUsernameAndEmailLikeThat(request.username, request.email);
            if (exists)
            {
                logger.LogWarning("Registration failed: Username or Email taken: {Username}", request.username);
                throw new BadRequestException("User with that username or email already exists");
            }
            UserDTO userModel = await userRepository.CreateUserWithDefaultRole(request.username, request.password, request.email, cancellationToken);
            var user = UserDetails.Create(userModel.Id, new Fullname(request.name, request.surname), new Email(request.email));
            await userDetailsRepository.AddAsync(user, cancellationToken);
            var accessToken = tokenService.GenerateAccessToken(user.Id, userModel.Username, userModel.Roles);
            var refreshToken = await tokenService.GenerateRefreshToken(user.Id, userModel.Username, cancellationToken);
            return new SignUpResponse(userModel.Username, accessToken, refreshToken);
        }
    }
}
