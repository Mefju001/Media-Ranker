using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Builder.Interfaces;
using WebApplication1.Data;
using WebApplication1.DTO.Mapper;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using WebApplication1.QueryHandler.Query;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class TvSeriesServices : ITvSeriesServices
    {
        private readonly IMediator mediatR;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITvSeriesBuilder builder;
        private readonly IReferenceDataService referenceDataService;
        public TvSeriesServices(IMediator mediatR, IUnitOfWork _unitOfWork, ITvSeriesBuilder builder, IReferenceDataService referenceDataService)
        {
            unitOfWork = _unitOfWork;
            this.builder = builder;
            this.referenceDataService = referenceDataService;
            this.mediatR = mediatR;
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
            return TvSeries.Select(TvSeriesMapper.ToTvSeriesResponse).ToList();
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
            return TvSeriesMapper.ToTvSeriesResponse(TvSeries);
        }



        public async Task<List<TvSeriesResponse>> GetMoviesByCriteriaAsync(TvSeriesQuery tvSeriesQuery)
        {
            var movies = await mediatR.Send(tvSeriesQuery);
            return movies;
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
            return TvSeries.Select(x => TvSeriesMapper.ToTvSeriesAVGResponse(x.TvSeries, x.avarage)).ToList();
        }
        public async Task<(int tvSeriesId, TvSeriesResponse response)> Upsert(int? tvSeriesId, TvSeriesRequest tvSeriesRequest)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var genre = await referenceDataService.GetOrCreateGenreAsync(tvSeriesRequest.genre);
                TvSeries? tvSeries;
                if (tvSeriesId is not null)
                {
                    tvSeries = await unitOfWork.TvSeries.AsQueryable()
                            .Include(m => m.genre)
                            .FirstOrDefaultAsync(m => m.Id == tvSeriesId.Value);
                    if (tvSeries is not null)
                    {
                        TvSeriesMapper.UpdateEntity(tvSeries, tvSeriesRequest, genre);
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
                var response = TvSeriesMapper.ToTvSeriesResponse(tvSeries);
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
