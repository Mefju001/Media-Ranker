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
                throw new NotFoundException("not found");
            }
            var movieResponse = MovieMapper.ToMovieResponse(movie);
            return movieResponse;
        }
    }
}
