using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.GetMyDetails
{
    internal class GetMyDetailsHandler : IRequestHandler<GetMyDetailsQuery, GetMyDetailsResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetMyDetailsHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<GetMyDetailsResponse?> Handle(GetMyDetailsQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<UserDetails>()
                .Where(u => u.Id == request.id)
                .AsNoTracking()
                .Select(u => new GetMyDetailsResponse
                (
                    u.Email.ToString(),
                    u.Fullname.FirstName,
                    u.Fullname.LastName
                ))
                .FirstOrDefaultAsync(cancellationToken);
            }
    }
}
