using Application.Common.Interfaces;
using Application.Features.AuthServices.Common;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Repository;
using Domain.Value_Object;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Features.AuthServices.Signup
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpResponse>
    {
        private readonly IIdentityService identityService;
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly ITokenService tokenService;
        private readonly ILogger<SignUpHandler> logger;
        public SignUpHandler(IIdentityService identityService, IUserDetailsRepository userDetailsRepository, ITokenService tokenService, ILogger<SignUpHandler> logger)
        {
            this.identityService = identityService;
            this.tokenService = tokenService;
            this.logger = logger;
            this.userDetailsRepository = userDetailsRepository;
        }

        public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            bool exists = await identityService.IsAnyUserWhoHaveEmailAndId(request.email, request.username, cancellationToken);
            if (exists)
            {
                logger.LogWarning("Registration failed: Username or Email taken: {Username}", request.username);
                throw new BadRequestException("User with that username or email already exists");
            }
            var userModel = await identityService.CreateUserWithDefaultRole(request.username, request.password, request.email);
            var user = UserDetails.Create(userModel.Id, new Fullname(request.name, request.surname), new Username(request.username), Email.Create(request.email));
            await userDetailsRepository.AddAsync(user, cancellationToken);
            var accessToken = tokenService.GenerateAccessToken(user.Id, userModel.Username, userModel.Roles);
            var refreshToken = await tokenService.GenerateRefreshToken(user.Id, userModel.Username, cancellationToken);
            return new SignUpResponse(userModel.Username, accessToken, refreshToken);
        }
    }
}
