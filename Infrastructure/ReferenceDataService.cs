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
        private IUnitOfWork unitOfWork;
        public ReferenceDataService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<DirectorDomain> GetOrCreateDirectorAsync(DirectorRequest directorRequest)
        {
            var Director = await unitOfWork.DirectorRepository.FirstOrDefaultForNameAndSurnameAsync(directorRequest.Name,directorRequest.Surname);
            if (Director is not null) return Director;
            Director = DirectorDomain.Create(directorRequest.Name, directorRequest.Surname);
            var result = await unitOfWork.DirectorRepository.AddAsync(Director);
            return result;
        }
        public async Task<GenreDomain> GetOrCreateGenreAsync(GenreRequest genreRequest)
        {
            var Genre = await unitOfWork.GenreRepository.FirstOrDefaultForNameAsync(genreRequest.name);
            if (Genre is not null) return Genre;
            Genre = GenreDomain.Create(genreRequest.name);
            var result = await unitOfWork.GenreRepository.AddAsync(Genre);
            return result;
        }
        public async Task<Dictionary<string,GenreDomain>> EnsureGenresExistAsync(List<string>names)
        {
            var existingGenres = await unitOfWork.GenreRepository.GetByNamesAsync(names);
            var genresMap = existingGenres.ToDictionary(g => g.name);
            foreach (var name in names)
            {
                if (!genresMap.ContainsKey(name))
                {
                    var newGenre = GenreDomain.Create(name);
                    genresMap.Add(name, newGenre);
                    await unitOfWork.GenreRepository.AddAsync(newGenre);
                }
            }
            return genresMap;
        }
        public async Task<List<GenreResponse>> GetGenres()
        {
            GenreDomain genres = null; //await unitOfWork.GenreRepository();
            if (genres is null) return new List<GenreResponse>();
            //var response = genres.Select(GenreMapper.ToResponse).ToList();
            // return response;
            throw new NotImplementedException();
        }
        public async Task saveToken(TokenDomain token)
        {
            if (token == null) throw new ArgumentNullException();
            await unitOfWork.TokenRepository.SaveToken(token);
            await unitOfWork.CompleteAsync();
        }

        public async Task<Dictionary<(string,string),DirectorDomain>> EnsureDirectorsExistAsync(List<DirectorRequest> directors)
        {
            var uniquePairs = directors.Select(d=> (Name: d.Name.Trim(), Surname: d.Surname.Trim())).Distinct().ToList();
            var existingDirectors = await unitOfWork.DirectorRepository.findByNames(uniquePairs);
            var directorMap = existingDirectors.ToDictionary(
               d => (d.name, d.surname));

            foreach (var pair in uniquePairs)
            {
                if (!directorMap.ContainsKey(pair))
                {
                    var newDirector = DirectorDomain.Create(pair.Name, pair.Surname);
                    await unitOfWork.DirectorRepository.AddAsync(newDirector);

                    directorMap.Add(pair, newDirector);
                }
            }

            return directorMap;
        }
    }
}
