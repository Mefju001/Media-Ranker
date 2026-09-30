using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.UserInteractions.Rankings.Get
{
    public record GetQuery(string type) : IQuery<List<RankingResponse>>;
}
