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
            var movies = await unitOfWork.MovieRepository.GetAllAsync(cancellationToken);
            var genres = await unitOfWork.GenreRepository.GetGenresDictionary();
            var directorsDict = await unitOfWork.DirectorRepository.GetDirectorsDictionary();
            var movieResponse = movies.Select(m =>
            {
                genres.TryGetValue(m.GenreId, out var genre);
                directorsDict.TryGetValue(m.DirectorId, out var director);
                return MovieMapper.ToMovieResponse(m,genre!,director!);
            }).ToList();
            return movieResponse;
        }
    }
}
