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

        public GetMovieByIdHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<MovieResponse?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await unitOfWork.MovieRepository.FirstOrDefaultAsync(request.id);
            if (movie == null)
            {
                throw new NotFoundException("Movie not found");
            }
            var genre = await unitOfWork.GenreRepository.Get(movie.GenreId);
            if (genre == null)
            {
                throw new NotFoundException("genre not found");
            }
            var director = await unitOfWork.DirectorRepository.Get(movie.DirectorId);
            if (director == null) {
                throw new NotFoundException("director not found");
            }
            var movieResponse = MovieMapper.ToMovieResponse(movie,genre,director);
            return movieResponse;
        }
    }
}
