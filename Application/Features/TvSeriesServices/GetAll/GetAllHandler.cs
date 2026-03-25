using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.TvSeriesServices.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllTvSeriesQuery, List<TvSeriesResponse>>
    {
        private readonly ITvSeriesRepository tvSeriesRepository;
        private readonly IGenreRepository genreRepository;
        public GetAllHandler(ITvSeriesRepository tvSeriesRepository, IGenreRepository genreRepository)
        {
            this.tvSeriesRepository = tvSeriesRepository;
            this.genreRepository = genreRepository;
        }
        public async Task<List<TvSeriesResponse>> Handle(GetAllTvSeriesQuery request, CancellationToken cancellationToken)
        {
            var tvSeries = await tvSeriesRepository.GetAll(cancellationToken);
            var genres = await genreRepository.GetGenresDictionary(cancellationToken);
            var tvSeriesResponses = tvSeries.Select(t =>
                TvSeriesMapper.ToTvSeriesResponse(t, genres[t.GenreId])).ToList();
            return tvSeriesResponses;
        }
    }
}
