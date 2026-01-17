using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;

namespace Application.Features.MovieServices.GetAll
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
            var movies = await unitOfWork.MovieRepository.GetAllAsync();
            var MovieResponse = movies.Select(m=>MovieMapper.ToMovieResponse(m,GenreDomain.Create(""),DirectorDomain.Create("",""))).ToList();
            return (MovieResponse);
        }
    }
}
