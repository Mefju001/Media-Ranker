using MediatR;
using WebApplication1.Application.Common.DTO.Response;

namespace WebApplication1.Application.Features.Movies.GetMoviesByCriteria
{
    public class GetMoviesByCriteriaQuery() : IRequest<List<MovieResponse>>
    {
        // PARAMETRY FILTROWANIA (Dla Wzorca Specyfikacja)

        public string? TitleSearch { get; set; }
        public double? MinRating { get; set; }
        public int? ReleaseYear { get; set; }
        public string? genreName { get; set; }
        public string? DirectorName { get; set; }
        public string? DirectorSurname { get; set; }


        // PARAMETRY SORTOWANIA (Dla Wzorca Strategia)

        public string? SortByField { get; set; }
        public bool IsDescending { get; set; } = false;
    }

}
