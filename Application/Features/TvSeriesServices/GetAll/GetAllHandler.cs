using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.TvSeriesServices.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllTvSeriesQuery, List<TvSeriesResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<TvSeriesResponse>> Handle(GetAllTvSeriesQuery request, CancellationToken cancellationToken)
        {
            var tvSeries = await unitOfWork.TvSeriesRepository.GetAll(cancellationToken);
            var genres = await unitOfWork.GenreRepository.GetAllAsync(cancellationToken);
            var genresDict = genres.ToDictionary(g => g.Id, g => g);
            var tvSeriesResponses = tvSeries.Select(t=> {
                genresDict.TryGetValue(t.GenreId, out var genreDomain);
                return TvSeriesMapper.ToTvSeriesResponse(t, genreDomain!);
                }).ToList();
            return (tvSeriesResponses);
        }
    }
}
