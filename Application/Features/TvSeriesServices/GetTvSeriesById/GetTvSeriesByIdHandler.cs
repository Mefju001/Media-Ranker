using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.TvSeriesServices.GetTvSeriesById
{
    public class GetTvSeriesByIdHandler : IRequestHandler<GetTvSeriesByIdQuery, TvSeriesResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITvSeriesRepository tvSeriesRepository;
        private readonly IGenreRepository genreRepository;

        public GetTvSeriesByIdHandler(IUnitOfWork unitOfWork, ITvSeriesRepository tvSeriesRepository, IGenreRepository genreRepository)
        {
            this.unitOfWork = unitOfWork;
            this.tvSeriesRepository = tvSeriesRepository;
            this.genreRepository = genreRepository;
        }

        public async Task<TvSeriesResponse?> Handle(GetTvSeriesByIdQuery request, CancellationToken cancellationToken)
        {
            var tvSeriesDomain = await tvSeriesRepository.GetTvSeriesById(request.id);
            if (tvSeriesDomain == null)
            {
                throw new NotFoundException("not found");
            }
            var genre = await genreRepository.Get(tvSeriesDomain.GenreId);
            if (genre == null)
            {
                throw new NotFoundException("not found");
            }
            var tvSeriesResponse = TvSeriesMapper.ToTvSeriesResponse(tvSeriesDomain, genre);
            return tvSeriesResponse;
        }
    }
}
