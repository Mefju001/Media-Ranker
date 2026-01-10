using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface ISorterContext<T> where T: class
    {
        IQueryable<T> Sort(IQueryable<T> query, string? SortByfield, bool isDescending);
    }
}
