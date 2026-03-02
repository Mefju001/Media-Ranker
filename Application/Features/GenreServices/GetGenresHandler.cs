using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using MediatR;

namespace Application.Features.GenreServices
{
    public class GetGenresHandler : IRequestHandler<GetGenresQuery, List<GenreResponse>>
    {
        private readonly IGenreRepository genreRepository;
        public GetGenresHandler(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public async Task<List<GenreResponse>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await genreRepository.GetAllAsync(cancellationToken);
            return genres.Select(GenreMapper.ToResponse).ToList();
        }
    }
}
