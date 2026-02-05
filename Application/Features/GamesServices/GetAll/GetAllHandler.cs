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
            var games = await unitOfWork.GameRepository.GetAllAsync();
            var genres = await unitOfWork.GenreRepository.GetGenresDictionary();
            var gameResponses = games.Select(g=>
                GameMapper.ToGameResponse(g, genres[g.GenreId])).ToList();
            return (gameResponses);
        }
    }
}
