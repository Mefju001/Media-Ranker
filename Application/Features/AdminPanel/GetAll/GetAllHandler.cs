using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using MediatR;

namespace Application.Features.AdminPanel.GetAll
{
    internal class GetAllHandler : IRequestHandler<GetAllQuery, GetAllResponse>
    {
        private readonly IAppDbContext appDbContext;
        public GetAllHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<GetAllResponse> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var numbersOfMovies = appDbContext.Medias.OfType<Movie>().Count();
            var numbersOfGames = appDbContext.Medias.OfType<Game>().Count();
            var numbersOfTvSeries = appDbContext.Medias.OfType<Domain.Aggregate.TvSeries>().Count();
            return new GetAllResponse(numbersOfGames, numbersOfMovies, numbersOfTvSeries);
        }
    }
}
