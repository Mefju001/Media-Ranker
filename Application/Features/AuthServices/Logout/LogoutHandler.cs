using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.AuthServices.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenRepository tokenRepository;
        private readonly IUserRepository userRepository;
        public LogoutHandler(IUnitOfWork unitOfWork, ITokenRepository tokenRepository, IUserRepository userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.tokenRepository = tokenRepository;
            this.userRepository = userRepository;
        }
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var resultOfTokens = await tokenRepository.DeleteTokensFromUserId(request.UserId);
            await unitOfWork.CompleteAsync();
        }
    }
}
