using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

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
