using Application.Features.Common.Interfaces;
using Application.Features.User.Common;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.GetById
{
    internal class GetByIdHandler : IRequestHandler<GetByIdQuery, UserDetailsResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetByIdHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<UserDetailsResponse?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<UserDetails>()
                .Where(x => x.Id == request.id)
                .Select(x => UserMapper.ToResponse(x))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
