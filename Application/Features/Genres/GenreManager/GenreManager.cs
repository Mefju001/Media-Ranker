using Application.Common.Interfaces;
using Application.Features.Genres.Common;
using Application.Features.Genres.GetAll;
using Domain.Aggregate;

namespace Application.Features.Genres.GenreManager
{
    internal class GenreManager:IGenreManager
    {
        private readonly IGenreRepository repository;
        public GenreManager(IGenreRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<GenreResponse>> GetOrCreateBatchAsync(List<string> names, CancellationToken cancellationToken)
        {
            var distinctNames = names.Where(n => !string.IsNullOrWhiteSpace(n)).Distinct().ToList();
            var existingGenres = await repository.GetByNamesAsync(distinctNames, cancellationToken);
            var existingNames = existingGenres.Select(g => g.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var nonExistingNames = new List<Genre>();
            foreach (var name in distinctNames)
            {
                if (!existingNames.Contains(name))
                {
                    var newGenre = Genre.Create(name);
                    nonExistingNames.Add(newGenre);
                }
            }
            await repository.AddRangeAsync(nonExistingNames, cancellationToken);
            return existingGenres.Select(GenreMapper.ToResponse).Concat(nonExistingNames.Select(GenreMapper.ToResponse)).ToList();
        }
        public async Task<GenreResponse> GetOrCreateAsync(GenreRequest genreRequest, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(genreRequest.name)) throw new ArgumentException("Genre name cannot be null or whitespace.", nameof(genreRequest.name));
            var Genre = await repository.FirstOrDefaultForNameAsync(genreRequest.name, cancellationToken);
            if (Genre is not null) return GenreMapper.ToResponse(Genre);
            Genre = Genre.Create(genreRequest.name);
            var result = await repository.AddAsync(Genre, cancellationToken);
            return GenreMapper.ToResponse(result);
        }
    }
}
