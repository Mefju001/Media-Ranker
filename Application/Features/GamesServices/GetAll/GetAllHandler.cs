using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.GamesServices.GetAll
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
            throw new NotImplementedException();
            /*var games = await unitOfWork.Games.GetAllAsync();
            var gameResponses = games.Select(GameMapper.ToGameResponse).ToList();
            return (gameResponses);*/
        }
    }
}
