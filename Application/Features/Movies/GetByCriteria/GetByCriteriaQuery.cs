using Application.Features.Common.Interfaces;
using Application.Features.Movies.Common;

namespace Application.Features.Movies.GetMoviesByCriteria
{
    public class GetByCriteriaQuery() : IQuery<List<MovieResponse>>
    {
        public string? TitleSearch { get; set; }
        public double? MinRating { get; set; }
        public int? ReleaseYear { get; set; }
        public string? genreName { get; set; }
        public string? DirectorName { get; set; }
        public string? DirectorSurname { get; set; }

        public string? SortByField { get; set; }
        public bool IsDescending { get; set; } = false;
    }

}
