using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;

namespace Application.Features.LikedServices.GetAllLiked
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<LikedMediaResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<LikedMediaResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var likedItems = await unitOfWork.LikedMediaRepository.GetAll();
            return likedItems.Select(LikedMediaMapper.ToResponse).ToList();
        }
    }
}
