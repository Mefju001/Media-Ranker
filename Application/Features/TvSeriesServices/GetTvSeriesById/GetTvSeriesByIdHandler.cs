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

        public GetTvSeriesByIdHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<TvSeriesResponse?> Handle(GetTvSeriesByIdQuery request, CancellationToken cancellationToken)
        {
            var tvSeriesDomain = await unitOfWork.TvSeriesRepository.GetTvSeriesById(request.id);
            if (tvSeriesDomain == null)
            {
                throw new NotFoundException("not found");
            }
            var genre = await unitOfWork.GenreRepository.Get(tvSeriesDomain.GenreId);
            if (genre == null)
            {
                throw new NotFoundException("not found");
            }
            var tvSeriesResponse = TvSeriesMapper.ToTvSeriesResponse(tvSeriesDomain, genre);
            return tvSeriesResponse;
        }
    }
}
