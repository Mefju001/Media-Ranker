using Application.Features.Common.Interfaces;

namespace Application.Features.User.GetMyDetails
{
    public record GetMyDetailsQuery(Guid id) : IQuery<GetMyDetailsResponse?>;
}
