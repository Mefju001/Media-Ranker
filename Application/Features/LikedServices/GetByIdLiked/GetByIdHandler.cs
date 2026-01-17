using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.LikedServices.GetByIdLiked
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, LikedMediaResponse?>
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
