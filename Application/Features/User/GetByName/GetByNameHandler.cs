using Application.Features.Common.Interfaces;
using Application.Features.User.Common;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.GetByName
{
    internal class GetByNameHandler : IRequestHandler<GetByNameQuery, UserDetailsResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetByNameHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<UserDetailsResponse?> Handle(GetByNameQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<UserDetails>()
                .Where(u => u.Fullname.FirstName == request.name)
                .AsNoTracking()
                .Select(u => UserMapper.ToResponse(u))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
