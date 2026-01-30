using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.GamesServices.GetGameById
{
    public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, GameResponse?>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetGameByIdHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<GameResponse?> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await unitOfWork.GameRepository.GetGameDomainAsync(request.id, cancellationToken);
            if (game == null)
            {
                throw new NotFoundException("not found");
            }
            var gameResponses = GameMapper.ToGameResponse(game);
            return gameResponses;
        }
    }
}
