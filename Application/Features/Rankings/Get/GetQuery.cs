using Application.Features.Common.Interfaces;
using MediatR;

namespace Application.Features.Rankings.Get
{
    public record GetQuery(string type) : IQuery<List<RankingResponse>>;
}
