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
            var Director = await _unitOfWork.DirectorRepository.FirstOrDefaultForNameAndSurnameAsync(directorRequest.Name,directorRequest.Surname);
            if (Director is not null) return Director;
            Director = DirectorDomain.Create(directorRequest.Name, directorRequest.Surname);
            var result = await _unitOfWork.DirectorRepository.AddAsync(Director);
            return result;
        }
        public async Task<GenreDomain> GetOrCreateGenreAsync(GenreRequest genreRequest)
        {
            var genre = await _unitOfWork.GenreRepository.FirstOrDefaultForNameAsync(genreRequest.name);
            if (genre != null) return genre;
            genre = GenreDomain.Create(genreRequest.name);
            var result = await _unitOfWork.GenreRepository.AddAsync(genre);
            return result;
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
