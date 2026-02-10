using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    public class GetMoviesByCriteriaHandler : IRequestHandler<GetMoviesByCriteriaQuery, List<MovieResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMovieSortAndFilterService SortAndFilterService;
        public GetMoviesByCriteriaHandler(IUnitOfWork appDbContext, IMovieSortAndFilterService sortAndFilterService)
        {
            unitOfWork = appDbContext;
            SortAndFilterService = sortAndFilterService;
        }

        public async Task<List<MovieResponse>> Handle(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = SortAndFilterService.ApplyFilters(request);
            query = SortAndFilterService.ApplySorting(query, request);
            var movies = await query.ToListAsync(cancellationToken);
            var genres = await unitOfWork.GenreRepository.GetGenresDictionary();
            var directors = await unitOfWork.DirectorRepository.GetDirectorsDictionary();
            var responses = movies.Select(m => {
                genres.TryGetValue(m.GenreId, out var genreDomain);
                directors.TryGetValue(m.DirectorId, out var directorDomain);
                return MovieMapper.ToMovieResponse(m, genreDomain!, directorDomain!);
            }).ToList();
            return responses;
        }
    }
}
