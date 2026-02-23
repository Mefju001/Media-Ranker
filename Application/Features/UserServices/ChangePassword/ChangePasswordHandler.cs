using Application.Common.Interfaces;
using Domain.DomainServices;
using Domain.Entity;
using Domain.Exceptions;
using Domain.Value_Object;
using MediatR;
using Microsoft.AspNetCore.Identity;



namespace Application.Features.UserServices.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;
        private readonly IUserPasswordService userPasswordService;

        public ChangePasswordHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IUserPasswordService userPasswordService)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.userPasswordService = userPasswordService;
        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.newPassword != request.confirmPassword)
                throw new PasswordMismatchException("The passwords provided are different.");
            var user = await userRepository.GetUserById(request.userId);
            if (user is null)
            {
                throw new InvalidCredentialsException("Invalid user or password.");
            }
            var password = userPasswordService.GenerateNewPassword(user,request.oldPassword,request.newPassword);
            user.ChangePassword(password);
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
