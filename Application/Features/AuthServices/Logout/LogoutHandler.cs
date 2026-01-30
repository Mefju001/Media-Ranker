using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.AuthServices.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public LogoutHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userId = ParseUserId(request.stringUserId);
            var result = await _unitOfWork.TokenRepository.DeleteTokensFromUserId(userId);
            if ((result == true))
            {
                await _unitOfWork.CompleteAsync();
            }
        }

        private int ParseUserId(string stringUserId)
        {
            if (!int.TryParse(stringUserId, out var userId))
                throw new InvalidOperationException("Parse from string to int");
            return userId;
        }
    }
}
