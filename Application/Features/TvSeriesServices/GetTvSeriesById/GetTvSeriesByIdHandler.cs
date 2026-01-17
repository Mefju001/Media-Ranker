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
            throw new NotImplementedException();
           /* var movie = await unitOfWork.TvSeries.GetByIdAsync(request.id);
            if (movie == null)
            {
                throw new NotFoundException("not found");
            }
            var movieResponse = TvSeriesMapper.ToTvSeriesResponse(movie);
            return movieResponse;*/
        }
    }
}
