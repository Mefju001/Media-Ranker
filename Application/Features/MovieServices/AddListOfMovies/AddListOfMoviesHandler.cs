using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MovieServices.AddListOfMovies
{
    public class AddListOfMoviesHandler : IRequestHandler<AddListOfMoviesCommand, List<MovieResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReferenceDataService referenceDataService;
        public AddListOfMoviesHandler(IReferenceDataService referenceDataService, IUnitOfWork unitOfWork)
        {
            this.referenceDataService = referenceDataService;
            this._unitOfWork = unitOfWork;
        }
        public async Task<List<MovieResponse>> Handle(AddListOfMoviesCommand requests, CancellationToken cancellationToken)
        {
            if (requests is null) throw new ArgumentNullException(nameof(requests));
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var moviesWithRef = new List<(MovieDomain,GenreDomain,DirectorDomain)>();
                foreach (var request in requests.requests)
                {
                    var director = await referenceDataService.GetOrCreateDirectorAsync(request.Director);
                    var genre = await referenceDataService.GetOrCreateGenreAsync(request.Genre);
                    var movie = MovieDomain.Create(request.Title, request.Description, request.Language,
                                                  request.ReleaseDate, genre.Id, director.Id,
                                                  request.Duration, request.IsCinemaRelease);

                    moviesWithRef.Add((movie,genre,director));
                }
                var movies = moviesWithRef.Select(m=>m.Item1).ToList();
                await _unitOfWork.MovieRepository.AddAsync(movies);
                await _unitOfWork.CompleteAsync();
                var listOfResponses = moviesWithRef.Select(m => MovieMapper.ToMovieResponse(m.Item1, m.Item2, m.Item3)).ToList();
                await transaction.CommitAsync();
                return listOfResponses;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
