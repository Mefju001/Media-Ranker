using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

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
