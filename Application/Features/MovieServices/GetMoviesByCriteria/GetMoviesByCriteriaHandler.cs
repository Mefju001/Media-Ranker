using Application.Common.DTO.Response;
using MediatR;


namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public class GetMoviesByCriteriaHandler : IRequestHandler<GetMoviesByCriteriaQuery, List<MovieResponse>>
    {
        private readonly IMovieSortAndFilterService SortAndFilterService;

        public GetMoviesByCriteriaHandler(IMovieSortAndFilterService sortAndFilterService)
        {
            SortAndFilterService = sortAndFilterService;
        }

        public async Task<List<MovieResponse>> Handle(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var responses = await SortAndFilterService.Handler(request,cancellationToken);
            
            return responses;
        }
    }
}
