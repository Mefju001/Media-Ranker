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
        private readonly IMovieSortAndFilterService SortAndFilterService;
        private readonly IMovieRepository movieRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;
        public GetMoviesByCriteriaHandler(IMovieSortAndFilterService sortAndFilterService,IMovieRepository movieRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository)
        {
            SortAndFilterService = sortAndFilterService;
            this.movieRepository = movieRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }

        public async Task<List<MovieResponse>> Handle(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = await SortAndFilterService.Handler(request);
            var movies = await movieRepository.GetListFromQuery(query,cancellationToken);
            var genres = await genreRepository.GetGenresDictionary();
            var directors = await directorRepository.GetDirectorsDictionary();
            var responses = movies.Select(m => {
                genres.TryGetValue(m.GenreId, out var genreDomain);
                directors.TryGetValue(m.DirectorId, out var directorDomain);
                return MovieMapper.ToMovieResponse(m, genreDomain!, directorDomain!);
            }).ToList();
            return responses;
        }
    }
}
