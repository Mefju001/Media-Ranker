using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Entity;

namespace Infrastructure
{
    public class ReferenceDataService : IReferenceDataService
    {
        private IUnitOfWork _unitOfWork;
        public ReferenceDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DirectorDomain> GetOrCreateDirectorAsync(DirectorRequest directorRequest)
        {
            DirectorDomain Director = null;//await _unitOfWork.Directors.FirstOrDefaultAsync(d => d.name == directorRequest.Name && d.surname == directorRequest.Surname);
            if (Director is not null) return DirectorDomain.Reconstruct(Director.Id, Director.name, Director.surname);
            //Director = new Director { name = directorRequest.Name, surname = directorRequest.Surname };
            //await _unitOfWork.Directors.AddAsync(Director);
            return DirectorDomain.Create(Director.name, Director.surname); ;
        }
        public async Task<GenreDomain> GetOrCreateGenreAsync(GenreRequest genreRequest)
        {
            GenreDomain genre = null;//await _unitOfWork.Genres.FirstOrDefaultAsync(g => g.name == genreRequest.name);
            if (genre != null) return GenreDomain.Reconstruct(genre.Id, genre.name);
            //genre = new Genre { name = genreRequest.name };
           // await _unitOfWork.Genres.AddAsync(genre);
            return GenreDomain.Reconstruct(genre.Id, genre.name);
        }
        public async Task<List<GenreResponse>> GetGenres()
        {
            GenreDomain genres = null;//await _unitOfWork.Genres.GetAllAsync();
            if (genres is null) return new List<GenreResponse>();
            //var response = genres.Select(GenreMapper.ToResponse).ToList();
            // return response;
            throw new NotImplementedException();
        }
        public async Task saveToken(TokenDomain token)
        {
            if (token == null) throw new ArgumentNullException();
            //await _unitOfWork.Tokens.AddAsync(token);
            await _unitOfWork.CompleteAsync();
        }
    }
}
