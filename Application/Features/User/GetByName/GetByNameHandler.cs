using Application.Features.Common.DTO.Response;
using Application.Features.Common.Interfaces;
using Application.Features.Common.Mapper;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.GetByName
{
    public class GetByNameHandler : IRequestHandler<GetByNameQuery, UserDetailsResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetByNameHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<UserDetailsResponse?> Handle(GetByNameQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<UserDetails>()
                .Where(u => u.Fullname.Name == request.name)
                .AsNoTracking()
                .Select(u => UserMapper.ToResponse(u))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
