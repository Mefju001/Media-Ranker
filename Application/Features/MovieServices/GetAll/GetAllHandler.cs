using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var movies = unitOfWork.MovieRepository.AsQueryable();
            var MovieResponse = await movies.Select(MovieMapper.ToDto).ToListAsync(cancellationToken);
            return (MovieResponse);
        }
    }
}
