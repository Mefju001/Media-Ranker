using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.MovieServices.GetMovieById
{
    public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, MovieResponse?>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMovieRepository movieRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;

        public GetMovieByIdHandler(IUnitOfWork unitOfWork, IMovieRepository movieRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository)
        {
            this.unitOfWork = unitOfWork;
            this.movieRepository = movieRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }

        public async Task<MovieResponse?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await movieRepository.FirstOrDefaultAsync(request.id);
            if (movie == null)
            {
                throw new NotFoundException("Movie not found");
            }
            var genre = await genreRepository.Get(movie.GenreId);
            if (genre == null)
            {
                throw new NotFoundException("genre not found");
            }
            var director = await directorRepository.Get(movie.DirectorId);
            if (director == null) {
                throw new NotFoundException("director not found");
            }
            var movieResponse = MovieMapper.ToMovieResponse(movie,genre,director);
            return movieResponse;
        }
    }
}
