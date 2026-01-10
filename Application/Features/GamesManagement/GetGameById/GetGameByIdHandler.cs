using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Exceptions;

namespace WebApplication1.Application.Features.Games.GetMovieById
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
            var game = await unitOfWork.Games.GetByIdAsync(request.id);
            if (game == null)
            {
                throw new NotFoundException("not found");
            }
            var gameResponses = GameMapper.ToGameResponse(game);
            return gameResponses;
        }
    }
}
