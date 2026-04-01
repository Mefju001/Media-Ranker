using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.MovieServices.GetMovieById
{
    public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, MovieResponse?>
    {
        private readonly IMediaRepository<Movie> mediaRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;

        public GetMovieByIdHandler(IMediaRepository<Movie> mediaRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository)
        {
            
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }

        public async Task<MovieResponse?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await mediaRepository.GetByIdAsync(request.id, cancellationToken);
            if (movie == null)
            {
                throw new NotFoundException("Movie not found");
            }
            var genre = await genreRepository.GetByIdAsync(movie.GenreId, cancellationToken);
            var director = await directorRepository.GetByIdAsync(movie.DirectorId, cancellationToken);


            if (genre == null) throw new NotFoundException("Genre not found");
            if (director == null) throw new NotFoundException("Director not found");
            return MovieMapper.ToMovieResponse(movie, genre, director);
        }
    }
}
