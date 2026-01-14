using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;

namespace Application.Features.LikedServices.GetByIdLiked
{
    public class GetByIdHandler:IRequestHandler<GetByIdQuery, LikedMediaResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetByIdHandler(IUnitOfWork unitOfWork) 
        {
           this.unitOfWork = unitOfWork;
        }

        public async Task<LikedMediaResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var liked = await unitOfWork.LikedMediaRepository.GetById(request.id);
            return liked == null ? null : LikedMediaMapper.ToResponse(liked);
        }
    }
}
