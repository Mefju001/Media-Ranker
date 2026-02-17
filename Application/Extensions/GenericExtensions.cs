using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class GenericExtensions
    {
        public static IQueryable<T> FilterByTitle<T>(this IQueryable<T> query, string? title)
        {
            throw new NotImplementedException();
            //return string.IsNullOrWhiteSpace(title) ? query : query.Where(q => q.Title);
        }
    }
}
///
/// Powtarza się tytuł,genre,minRating, releaseDate/ReleaseYear
///