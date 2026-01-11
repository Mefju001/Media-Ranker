using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Exceptions;

namespace WebApplication1.Application.Features.TvSeries.GetTvSeriesById
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
            var movie = await unitOfWork.TvSeries.GetByIdAsync(request.id);
            if (movie == null)
            {
                throw new NotFoundException("not found");
            }
            var movieResponse = TvSeriesMapper.ToTvSeriesResponse(movie);
            return movieResponse;
        }
    }
}
