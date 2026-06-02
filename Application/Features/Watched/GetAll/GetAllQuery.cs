using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;

namespace Application.Features.Watched.GetAll
{
    public record GetAllQuery(Guid userId):IQuery<List<UserInteractionsMapper>>;
}
