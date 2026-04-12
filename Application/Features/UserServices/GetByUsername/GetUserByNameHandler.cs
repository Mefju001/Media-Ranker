using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserServices.GetBy
{
    public class GetUserByNameHandler : IRequestHandler<GetUserByNameQuery, UserDetailsResponse?>
    {
        private readonly IAppDbContext appDbContext;
        public GetUserByNameHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<UserDetailsResponse?> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Set<UserDetails>()
                .Where(u=>u.Fullname.Name == request.name)
                .AsNoTracking()
                .Select(u=>UserMapper.ToResponse(u))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
