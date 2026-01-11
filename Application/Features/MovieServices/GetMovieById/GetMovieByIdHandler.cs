using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Exceptions;

namespace WebApplication1.Application.Features.Movies.GetMovieById
{
    public class GetMovieByIdHandler : IRequestHandler<GetTvSeriesByIdQuery, MovieResponse?>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetMovieByIdHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<MovieResponse?> Handle(GetTvSeriesByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await unitOfWork.Movies.GetByIdAsync(request.id);
            if (movie == null)
            {
                throw new NotFoundException("not found");
            }
            var movieResponse = MovieMapper.ToMovieResponse(movie);
            return movieResponse;
        }
    }
}
