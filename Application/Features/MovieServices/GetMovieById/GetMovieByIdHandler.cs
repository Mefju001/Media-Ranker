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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediaRepository mediaRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IDirectorRepository directorRepository;

        public GetMovieByIdHandler(IUnitOfWork unitOfWork, IMediaRepository mediaRepository, IGenreRepository genreRepository, IDirectorRepository directorRepository)
        {
            this.unitOfWork = unitOfWork;
            this.mediaRepository = mediaRepository;
            this.genreRepository = genreRepository;
            this.directorRepository = directorRepository;
        }

        public async Task<MovieResponse?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await mediaRepository.GetByIdAsync<Movie>(request.id, cancellationToken);
            if (movie == null)
            {
                throw new NotFoundException("Movie not found");
            }
            var genre = await genreRepository.Get(movie.GenreId, cancellationToken);
            var director = await directorRepository.Get(movie.DirectorId, cancellationToken);


            if (genre == null) throw new NotFoundException("Genre not found");
            if (director == null) throw new NotFoundException("Director not found");
            return MovieMapper.ToMovieResponse(movie, genre, director);
        }
    }
}
