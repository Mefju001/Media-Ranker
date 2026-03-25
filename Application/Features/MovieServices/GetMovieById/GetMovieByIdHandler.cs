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
            var movie = await movieRepository.FirstOrDefaultAsync(request.id, cancellationToken);
            if (movie == null)
            {
                throw new NotFoundException("Movie not found");
            }
            var genreTask = genreRepository.Get(movie.GenreId, cancellationToken);
            var directorTask = directorRepository.Get(movie.DirectorId, cancellationToken);

            await Task.WhenAll(genreTask, directorTask);

            var genre = await genreTask;
            var director = await directorTask;

            if (genre == null) throw new NotFoundException("Genre not found");
            if (director == null) throw new NotFoundException("Director not found");
            return MovieMapper.ToMovieResponse(movie, genre, director);
        }
    }
}
