using Application.Features.Common.Interfaces;
using Application.Features.Medias.Movies.Common;

namespace Application.Features.Medias.Movies.GetById
{
    public record GetByIdQuery(Guid id) : IQuery<MovieResponse?>;

}
