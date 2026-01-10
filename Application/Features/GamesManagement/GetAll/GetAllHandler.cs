using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;

namespace WebApplication1.Application.Features.Games.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<GameResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<GameResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var games = await unitOfWork.Games.GetAllAsync();
            var gameResponses = games.Select(GameMapper.ToGameResponse).ToList();
            return (gameResponses);
        }
    }
}
