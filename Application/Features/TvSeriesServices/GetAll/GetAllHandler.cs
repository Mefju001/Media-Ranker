using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.TvSeriesServices.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllTvSeriesQuery, List<TvSeriesResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITvSeriesRepository tvSeriesRepository;
        private readonly IGenreRepository genreRepository;
        public GetAllHandler(IUnitOfWork unitOfWork, ITvSeriesRepository tvSeriesRepository, IGenreRepository genreRepository)
        {
            this.unitOfWork = unitOfWork;
            this.tvSeriesRepository = tvSeriesRepository;
            this.genreRepository = genreRepository;
        }
        public async Task<List<TvSeriesResponse>> Handle(GetAllTvSeriesQuery request, CancellationToken cancellationToken)
        {
            var tvSeries = await tvSeriesRepository.GetAll(cancellationToken);
            var genres = await genreRepository.GetGenresDictionary();
            var tvSeriesResponses = tvSeries.Select(t =>
            {
                genres.TryGetValue(t.GenreId, out var genreDomain);
                return TvSeriesMapper.ToTvSeriesResponse(t, genreDomain!);
            }).ToList();
            return (tvSeriesResponses);
        }
    }
}
