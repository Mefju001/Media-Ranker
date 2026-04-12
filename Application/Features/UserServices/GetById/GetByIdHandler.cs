using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserServices.GetById
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, UserDetailsResponse?>
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
