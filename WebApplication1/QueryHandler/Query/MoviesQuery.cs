using MediatR;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.QueryHandler.Query
{
    public class MoviesQuery:IRequest<List<MovieResponse>>
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
