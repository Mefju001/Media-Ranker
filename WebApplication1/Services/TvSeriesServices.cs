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

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TvSeriesResponse>> GetAllAsync()
        {
            var TvSeries = await context.TvSeries
                                .Include(m => m.genre)
                                .Include(m => m.director)
                                .Include(m => m.Reviews)
                                .ThenInclude(r => r.User)
                                .ToListAsync();
            return TvSeries.Select(TvSeriesMapping.ToResponse).ToList();
        }

        public async Task<TvSeriesResponse> GetById(int id)
        {
            var TvSeries = await context.TvSeries
                .Include(m => m.genre)
                .Include(m => m.director)
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
                                .Include(m => m.director)
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
                                .Include(m => m.director)
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
            if (!string.IsNullOrEmpty(directorName))
            {
                query = query.Where(tv => tv.director.name.Contains(directorName) || tv.director.surname.Contains(directorName));
            }
            var movies = await query.ToListAsync();
            return movies.Select(TvSeriesMapping.ToResponse).ToList();
        }

        public async Task<List<TvSeriesResponse>> GetTvSeriesByAvrRating()
        {
            var TvSeries = await context.TvSeries
                    .Include(m => m.genre)
                    .Include(m => m.director)
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

        public Task<(int tvSeriesId, TvSeriesResponse response)> Upsert(int? tvSeriesId, TvSeriesRequest tvSeriesRequest)
        {
            throw new NotImplementedException();
        }
    }
}
