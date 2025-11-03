using Microsoft.EntityFrameworkCore;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using WebApplication1.QueryHandler;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class TvSeriesServices : ITvSeriesServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITvSeriesBuilder builder;
        private readonly QueryServices<TvSeries> handler;
        public TvSeriesServices(IUnitOfWork _unitOfWork, ITvSeriesBuilder builder,QueryServices<TvSeries>handler)
        {
            unitOfWork = _unitOfWork;
            this.builder = builder;
            this.handler = handler;
        }

        public async Task<bool> Delete(int id)
        {
            var TvSeries = await unitOfWork.TvSeries.FirstOrDefaultAsync(x => x.Id == id);
            if (TvSeries != null)
            {
                 unitOfWork.TvSeries.Delete(TvSeries);
                await unitOfWork.CompleteAsync();
                return true;
            }
            return false;
        }

        public async Task<List<TvSeriesResponse>> GetAllAsync()
        {
            var TvSeries = await unitOfWork.TvSeries.AsQueryable()
                                .Include(m => m.genre)
                                .Include(m => m.Reviews)
                                .ThenInclude(r => r.User)
                                .ToListAsync();
            return TvSeries.Select(TvSeriesMapping.ToTvSeriesResponse).ToList();
        }

        public async Task<TvSeriesResponse> GetById(int id)
        {
            var TvSeries = await unitOfWork.TvSeries.AsQueryable()
                .Include(m => m.genre)
                .Include(m => m.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(tv => tv.Id == id);
            if (TvSeries == null)
                throw new NotFoundException("Not found");
            return TvSeriesMapping.ToTvSeriesResponse(TvSeries);
        }

        public async Task<List<TvSeriesResponse>> GetSortAll(string sortDirection,string sortByfield)
        {
            var query = unitOfWork.TvSeries.AsQueryable()
                                .Include(m => m.genre)
                                .Include(m => m.Reviews)
                                .ThenInclude(r => r.User)
                                .AsQueryable();
            if(string.IsNullOrEmpty(sortByfield)||string.IsNullOrEmpty(sortDirection))
            {
                var isDesceding = sortDirection.Equals("desc",StringComparison.OrdinalIgnoreCase);
                query = handler.Handle(sortByfield, isDesceding);
                var TvSeries = await query.ToListAsync();
                return TvSeries.Select(x => TvSeriesMapping.ToTvSeriesResponse(x)).ToList();
            }
            throw new NullReferenceException();
        }

        public async Task<List<TvSeriesResponse>> GetTvSeries(string? name, string? genreName, string? directorName)
        {
            var query = unitOfWork.TvSeries.AsQueryable()
                                .Include(m => m.genre)
                                .Include(m => m.Reviews)
                                .ThenInclude(r => r.User)
                                .AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(tv => tv.title.Contains(name));
            }
            if (!string.IsNullOrEmpty(genreName))
            {
                query = query.Where(tv => tv.genre.name.Contains(genreName));
            }
            var movies = await query.ToListAsync();
            return movies.Select(TvSeriesMapping.ToTvSeriesResponse).ToList();
        }
        //modif
        public async Task<List<TvSeriesAVGResponse>> GetTvSeriesByAvrRating()
        {
            var TvSeries = await unitOfWork.TvSeries.AsQueryable()
                    .Include(m => m.genre)
                    .Include(m => m.Reviews)
                        .ThenInclude(r => r.User)
                    .Select(tv => new
                    {
                        TvSeries = tv,
                        avarage = tv.Reviews.Average(r => (double?)r.Rating) ?? 0
                    })
                    .OrderByDescending(x => x.avarage)
                    .ToListAsync();
            return TvSeries.Select(x => TvSeriesMapping.ToTvSeriesAVGResponse(x.TvSeries,x.avarage)).ToList();
        }

        private async Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest)
        {
            var genre = await unitOfWork.Genres.FirstOrDefaultAsync(g => g.name == genreRequest.name);
            if (genre is not null) return genre;
            genre = new Genre { name = genreRequest.name };
            await unitOfWork.Genres.AddAsync(genre);
            return genre;
        }
        public async Task<(int tvSeriesId, TvSeriesResponse response)> Upsert(int? tvSeriesId, TvSeriesRequest tvSeriesRequest)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var genre = await GetOrCreateGenreAsync(tvSeriesRequest.genre);
                TvSeries? tvSeries;
                if (tvSeriesId is not null)
                {
                    tvSeries = await unitOfWork.TvSeries.AsQueryable()
                            .Include(m => m.genre)
                            .FirstOrDefaultAsync(m => m.Id == tvSeriesId.Value);
                    if (tvSeries is not null)
                    {
                        TvSeriesMapping.UpdateEntity(tvSeries, tvSeriesRequest, genre);
                    }
                }
                else
                {
                    tvSeries = builder.CreateNew(tvSeriesRequest.title, tvSeriesRequest.description)
                        .WithGenre(genre)
                        .WithMetadata
                        (tvSeriesRequest.Seasons,
                        tvSeriesRequest.Episodes,
                        tvSeriesRequest.Network,
                        tvSeriesRequest.Status)
                        .Build();
                    await unitOfWork.TvSeries.AddAsync(tvSeries);
                }
                await unitOfWork.CompleteAsync();
                if (tvSeries is null) throw new ArgumentNullException(nameof(tvSeries));
                var response = TvSeriesMapping.ToTvSeriesResponse(tvSeries);
                await transaction.CommitAsync();
                return (tvSeries.Id, response);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
