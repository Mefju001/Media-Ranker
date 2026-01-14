using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Domain.Exceptions;

namespace Application.Features.UserServices.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteUserHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request.id < 0) throw new ArgumentOutOfRangeException("id");
            await unitOfWork.UserRepository.DeleteUser(request.id);
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
