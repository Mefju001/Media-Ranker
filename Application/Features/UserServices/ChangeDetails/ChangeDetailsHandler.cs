using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Domain.Exceptions;

namespace Application.Features.UserServices.ChangeDetails
{
    public class ChangeDetailsHandler : IRequestHandler<ChangeDetailsCommand, Unit>
    {
        private readonly IUnitOfWork unitOfWork;
        public ChangeDetailsHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(ChangeDetailsCommand request, CancellationToken cancellationToken)
        {
            if (request.userId < 0) throw new ArgumentOutOfRangeException("userId");
            if (request == null)
                throw new ArgumentException("you should fill that field");
            var user = await unitOfWork.UserRepository.GetUserById(request.userId) ?? throw new UserNotFoundException("Not found user");
            var emailExist = await unitOfWork.UserRepository.IsAnyUserWhoHaveEmailAndId(request.email, request.userId);
            if (emailExist) throw new EmailAlreadyExistsException("This email is taken.");
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
