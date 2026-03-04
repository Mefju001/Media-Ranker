using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;

namespace Infrastructure
{
    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDirectorRepository directorRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ITokenRepository tokenRepository;
        public ReferenceDataService(IUnitOfWork unitOfWork, IDirectorRepository directorRepository, IGenreRepository genreRepository, ITokenRepository tokenRepository)
        {
            this.unitOfWork = unitOfWork;
            this.directorRepository = directorRepository;
            this.genreRepository = genreRepository;
            this.tokenRepository = tokenRepository;
        }
        public async Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest, CancellationToken cancellationToken)
        {
            var Director = await directorRepository.FirstOrDefaultForNameAndSurnameAsync(directorRequest.Name, directorRequest.Surname, cancellationToken);
            if (Director is not null) return Director;
            Director = Director.Create(directorRequest.Name, directorRequest.Surname);
            var result = await directorRepository.AddAsync(Director, cancellationToken);
            return result;
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
            var genresMap = existingGenres.ToDictionary(g => g.name.Value);
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
        public async Task<List<GenreResponse>> GetGenres(CancellationToken cancellationToken)
        {
            var genres = await genreRepository.GetAllAsync(cancellationToken);
            if (genres is null) return new List<GenreResponse>();
            var response = genres.Select(GenreMapper.ToResponse).ToList();
            return response;
        }
        public async Task saveToken(Token token, CancellationToken cancellationToken)
        {
            if (token == null) throw new ArgumentNullException();
            await tokenRepository.SaveToken(token, cancellationToken);
            await unitOfWork.CompleteAsync();
        }

        public async Task<Dictionary<(string, string), Director>> EnsureDirectorsExistAsync(List<DirectorRequest> directors, CancellationToken cancellationToken)
        {
            var uniquePairs = directors
                .Select(d => (Name: d.Name.Trim(), Surname: d.Surname.Trim()))
                .Distinct()
                .ToList();
            var existingDirectors = await directorRepository.findByNames(uniquePairs, cancellationToken);
            var directorMap = existingDirectors.ToDictionary(
               d => (d.name.Trim(), d.surname.Trim()));
            foreach (var pair in uniquePairs)
            {
                if (!directorMap.ContainsKey(pair))
                {
                    var newDirector = Director.Create(pair.Name, pair.Surname);
                    await directorRepository.AddAsync(newDirector, cancellationToken);
                    directorMap.Add(pair, newDirector);
                }
            }
            return directorMap;
        }
    }
}
