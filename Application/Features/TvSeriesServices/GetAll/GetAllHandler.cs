using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using MediatR;

namespace Application.Features.TvSeriesServices.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllTvSeriesQuery, List<TvSeriesResponse>>
    {
        private readonly IMediaRepository mediaRepository;
        private readonly IGenreRepository genreRepository;
        public GetAllHandler(IMediaRepository mediaRepository, IGenreRepository genreRepository)
        {
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
        }
        public async Task<List<TvSeriesResponse>> Handle(GetAllTvSeriesQuery request, CancellationToken cancellationToken)
        {
            var tvSeries = await mediaRepository.GetAll<TvSeries>(cancellationToken);
            var genres = await genreRepository.GetGenresDictionary(cancellationToken);
            var tvSeriesResponses = tvSeries.Select(t =>
                TvSeriesMapper.ToTvSeriesResponse(t, genres[t.GenreId])).ToList();
            return tvSeriesResponses;
        }
    }
}
