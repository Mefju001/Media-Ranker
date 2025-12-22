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
        public async Task<List<TvSeriesResponse>>AddListOfTvSeries(List<TvSeriesRequest>tvSeriesRequests)
        {
            if(tvSeriesRequests is null) throw new ArgumentNullException(nameof(tvSeriesRequests));
            await using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                List<TvSeries> listTvSeries = new List<TvSeries>();
                foreach (var request in tvSeriesRequests)
                {
                    var genre = await referenceDataService.GetOrCreateGenreAsync(request.genre);
                    var tvSeries = builder.CreateNew(request.title, request.description)
                        .WithGenre(genre)
                        .WithMetadata
                        (request.Seasons,
                        request.Episodes,
                        request.Network,
                        request.Status)
                        .Build();
                    listTvSeries.Add(tvSeries);
                }
                await unitOfWork.TvSeries.AddRangeAsync(listTvSeries);
                await unitOfWork.CompleteAsync();
                var listOfResponses = listTvSeries.Select(TvSeriesMapper.ToTvSeriesResponse).ToList();
                await transaction.CommitAsync();
                return listOfResponses;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
            var TvSeries = await unitOfWork.TvSeries.GetAllAsync();
            return TvSeries.Select(TvSeriesMapper.ToTvSeriesResponse).ToList();
        }

        public async Task<TvSeriesResponse?> GetById(int id)
        {
            var TvSeries = await unitOfWork.TvSeries
                .FirstOrDefaultAsync(tv => tv.Id == id);
            if (TvSeries == null)
                return null;
            return TvSeriesMapper.ToTvSeriesResponse(TvSeries);
        }
        public async Task<List<TvSeriesResponse>> GetMoviesByCriteriaAsync(TvSeriesQuery tvSeriesQuery)
        {
            var movies = await mediatR.Send(tvSeriesQuery);
            return movies;
        }
        public async Task<TvSeriesResponse> Upsert(int? tvSeriesId, TvSeriesRequest tvSeriesRequest)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                var genre = await referenceDataService.GetOrCreateGenreAsync(tvSeriesRequest.genre);
                TvSeries? tvSeries;
                if (tvSeriesId is not null)
                {
                    tvSeries = await unitOfWork.TvSeries
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
                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
