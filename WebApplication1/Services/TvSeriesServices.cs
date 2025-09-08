using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class TvSeriesServices:ITvSeriesServices
    {
        private readonly AppDbContext context;
        public TvSeriesServices(AppDbContext _context)
        {
            this.context = _context;
        }

        public async Task<bool> Delete(int id)
        {
            var TvSeries = context.TvSeries.FirstOrDefault(x => x.Id == id);
                if(TvSeries!=null)
                {
                    context.TvSeries.Remove(TvSeries);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
        }

        public async Task<List<TvSeriesResponse>> GetAllAsync()
        {
            var TvSeries = await context.TvSeries
                                .Include(m => m.genre)
                                .Include(m => m.Reviews)
                                .ThenInclude(r => r.User)
                                .ToListAsync();
            return TvSeries.Select(TvSeriesMapping.ToResponse).ToList();
        }

        public async Task<TvSeriesResponse> GetById(int id)
        {
            var TvSeries = await context.TvSeries
                .Include(m => m.genre)
                .Include(m => m.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(tv => tv.Id == id);
            if (TvSeries == null)
                return null;
            return TvSeriesMapping.ToResponse(TvSeries);
        }

        public async Task<List<TvSeriesResponse>> GetSortAll(string sort)
        {
            sort = sort.ToLower();
            var query = context.TvSeries
                                .Include(m => m.genre)
                                .Include(m => m.Reviews)
                                .ThenInclude(r => r.User)
                                .AsQueryable();
            if(!string.IsNullOrEmpty(sort)&&sort =="asc")
            {
                query = query.OrderBy(tv => tv.title);
            }
            if(!string.IsNullOrEmpty(sort) && sort == "desc")
            {
                query = query.OrderByDescending(tv => tv.title);
            }
            var TvSeries = await query.ToListAsync();
            return TvSeries.Select(TvSeriesMapping.ToResponse).ToList();
        }

        public async Task<List<TvSeriesResponse>> GetTvSeries(string? name, string? genreName, string? directorName)
        {
            var query = context.TvSeries
                                .Include(m => m.genre)
                                .Include(m => m.Reviews)
                                .ThenInclude(r => r.User)
                                .AsQueryable();
            if(!string.IsNullOrEmpty(name))
            {
                query = query.Where(tv => tv.title.Contains(name));
            }
            if (!string.IsNullOrEmpty(genreName))
            {
                query = query.Where(tv => tv.genre.name.Contains(genreName));
            }
            var movies = await query.ToListAsync();
            return movies.Select(TvSeriesMapping.ToResponse).ToList();
        }

        public async Task<List<TvSeriesResponse>> GetTvSeriesByAvrRating()
        {
            var TvSeries = await context.TvSeries
                    .Include(m => m.genre)
                    .Include(m => m.Reviews)
                        .ThenInclude(r => r.User)
                    .Select(tv => new
                    {
                        TvSeries = tv,
                        avarage = tv.Reviews.Average(r=>(double?)r.Rating)??0
                    })
                    .OrderByDescending(x=>x.avarage)
                    .ToListAsync();
            return TvSeries.Select(x => TvSeriesMapping.ToResponse(x.TvSeries)).ToList();
        }

        private async Task<Genre> GetOrCreateGenreAsync(GenreRequest genreRequest)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(g => g.name == genreRequest.name);
            if (genre is not null) return genre;
            genre = new Genre { name = genreRequest.name };
            context.Genres.Add(genre);
            return genre;
        }
        public async Task<(int tvSeriesId, TvSeriesResponse response)> Upsert(int? tvSeriesId, TvSeriesRequest tvSeriesRequest)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var genre = await GetOrCreateGenreAsync(tvSeriesRequest.genre);
                TvSeries? tvSeries;
                if (tvSeriesId is not null)
                {
                    tvSeries = await context.TvSeries
                            .Include(m => m.genre)
                            .FirstOrDefaultAsync(m => m.Id == tvSeriesId.Value);
                    if (tvSeries is not null)
                    {
                        tvSeries.title = tvSeriesRequest.title;
                        tvSeries.description = tvSeriesRequest.description;
                        tvSeries.genre = genre;
                        tvSeries.ReleaseDate = tvSeriesRequest.ReleaseDate;
                        tvSeries.Language = tvSeriesRequest.Language;
                        tvSeries.Seasons = tvSeriesRequest.Seasons;
                        tvSeries.Episodes = tvSeriesRequest.Episodes;
                        tvSeries.Network = tvSeriesRequest.Network;
                        tvSeries.Status = tvSeriesRequest.Status;
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return (tvSeries.Id, TvSeriesMapping.ToResponse(tvSeries));
                    }
                }
                tvSeries = new TvSeries
                {
                    title = tvSeriesRequest.title,
                    description = tvSeriesRequest.description,
                    genre = genre,
                    ReleaseDate = tvSeriesRequest.ReleaseDate,
                    Language = tvSeriesRequest.Language,
                    Seasons = tvSeriesRequest.Seasons,
                    Episodes = tvSeriesRequest.Episodes,
                    Network = tvSeriesRequest.Network,
                    Status = tvSeriesRequest.Status
                };
                context.TvSeries.Add(tvSeries);
                await context.SaveChangesAsync();
                var response = TvSeriesMapping.ToResponse(tvSeries);
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
