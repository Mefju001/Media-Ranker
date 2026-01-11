using Microsoft.EntityFrameworkCore;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Common.Interfaces;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Entities;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class ReferenceDataService : IReferenceDataService
    {
        private IUnitOfWork _unitOfWork;
        public ReferenceDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest)
        {
            var Director = await _unitOfWork.Directors.FirstOrDefaultAsync(d => d.name == directorRequest.Name && d.surname == directorRequest.Surname);
            if (Director is not null) return Director;
            Director = new Director { name = directorRequest.Name, surname = directorRequest.Surname };
            await _unitOfWork.Directors.AddAsync(Director);
            return Director;
        }
        public async Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest)
        {
            var genre = await _unitOfWork.Genres.FirstOrDefaultAsync(g => g.name == genreRequest.name);
            if (genre != null) return genre;
            genre = new Genre { name = genreRequest.name };
            await _unitOfWork.Genres.AddAsync(genre);
            return genre;
        }
        public async Task<List<GenreResponse>> GetGenres()
        {
            var genres = await _unitOfWork.Genres.GetAllAsync();
            if (genres is null) return new List<GenreResponse>();
            var response = genres.Select(GenreMapper.ToResponse).ToList();
            return response;
        }
        public async Task saveTokens(Token token)
        {
            if (token == null) throw new ArgumentNullException();
            await _unitOfWork.Tokens.AddAsync(token);
            await _unitOfWork.CompleteAsync();
        }
    }
}
