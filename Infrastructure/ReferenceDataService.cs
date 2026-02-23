using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Entity;
using Infrastructure.Persistence.UnitOfWork;
using MediatR;

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
        public async Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest)
        {
            var Director = await directorRepository.FirstOrDefaultForNameAndSurnameAsync(directorRequest.Name,directorRequest.Surname);
            if (Director is not null) return Director;
            Director = Director.Create(directorRequest.Name, directorRequest.Surname);
            var result = await directorRepository.AddAsync(Director);
            return result;
        }
        public async Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest)
        {
            var Genre = await genreRepository.FirstOrDefaultForNameAsync(genreRequest.name);
            if (Genre is not null) return Genre;
            Genre = Genre.Create(genreRequest.name);
            var result = await genreRepository.AddAsync(Genre);
            return result;
        }
        public async Task<Dictionary<string,Genre>> EnsureGenresExistAsync(List<string>names)
        {
            var existingGenres = await genreRepository.GetByNamesAsync(names);
            var genresMap = existingGenres.ToDictionary(g => g.name.Value);
            foreach (var name in names)
            {
                if (!genresMap.ContainsKey(name))
                {
                    var newGenre = Genre.Create(name);
                    genresMap.Add(name, newGenre);
                    await genreRepository.AddAsync(newGenre);
                }
            }
            return genresMap;
        }
        public async Task<List<GenreResponse>> GetGenres()
        {
            Genre genres = null; //await unitOfWork.GenreRepository();
            if (genres is null) return new List<GenreResponse>();
            //var response = genres.Select(GenreMapper.ToResponse).ToList();
            // return response;
            throw new NotImplementedException();
        }
        public async Task saveToken(Token token)
        {
            if (token == null) throw new ArgumentNullException();
            await tokenRepository.SaveToken(token);
            await unitOfWork.CompleteAsync();
        }

        public async Task<Dictionary<(string,string),Director>> EnsureDirectorsExistAsync(List<DirectorRequest> directors)
        {
            var uniquePairs = directors.Select(d=> (Name: d.Name.Trim(), Surname: d.Surname.Trim())).Distinct().ToList();
            var existingDirectors = await directorRepository.findByNames(uniquePairs);
            var directorMap = existingDirectors.ToDictionary(
               d => (d.name, d.surname));

            foreach (var pair in uniquePairs)
            {
                if (!directorMap.ContainsKey(pair))
                {
                    var newDirector = Director.Create(pair.Name, pair.Surname);
                    await directorRepository.AddAsync(newDirector);

                    directorMap.Add(pair, newDirector);
                }
            }

            return directorMap;
        }
    }
}
