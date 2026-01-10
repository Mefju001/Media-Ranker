using MediatR;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;

namespace WebApplication1.Application.Features.Movies.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, List<MovieResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<MovieResponse>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var movies = await unitOfWork.Movies.GetAllAsync();
            var MovieResponse = movies.Select(MovieMapper.ToMovieResponse).ToList();
            return (MovieResponse);
        }
    }
}
