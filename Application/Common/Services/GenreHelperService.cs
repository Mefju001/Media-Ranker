using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Domain.Aggregate;

namespace Application.Common.Services
{
    public class GenreHelperService: IGenreHelperService
    {
        private readonly IGenreRepository genreRepository;
        public GenreHelperService(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }
        public async Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest, CancellationToken cancellationToken)
        {
            var Genre = await genreRepository.FirstOrDefaultForNameAsync(genreRequest.name, cancellationToken);
            if (Genre is not null) return Genre;
            Genre = Genre.Create(genreRequest.name);
            var result = await genreRepository.AddAsync(Genre, cancellationToken);
            return result;
        }
        public async Task<Dictionary<string, Genre>> EnsureGenresExistAsync(List<string> names, CancellationToken cancellationToken)
        {
            var existingGenres = await genreRepository.GetByNamesAsync(names, cancellationToken);
            var genresMap = existingGenres.ToDictionary(g => g.Name.Value);
            foreach (var name in names)
            {
                if (!genresMap.ContainsKey(name))
                {
                    var newGenre = Genre.Create(name);
                    genresMap.Add(name, newGenre);
                    await genreRepository.AddAsync(newGenre, cancellationToken);
                }
            }
            return genresMap;
        }
    }
}
