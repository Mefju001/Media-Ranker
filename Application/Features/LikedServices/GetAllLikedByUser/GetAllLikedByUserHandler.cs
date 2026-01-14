using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;

namespace Application.Features.LikedServices.GetAllLikedByUser
{
    public class GetAllLikedByUserHandler : IRequestHandler<GetAllLikedByUserQuery, List<LikedMediaResponse>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllLikedByUserHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<LikedMediaResponse>> Handle(GetAllLikedByUserQuery request, CancellationToken cancellationToken)
        {
            var results = await unitOfWork.LikedMediaRepository.GetLikedForUser(request.userId);
            return results.Select(LikedMediaMapper.ToResponse).ToList();
        }
    }
}
