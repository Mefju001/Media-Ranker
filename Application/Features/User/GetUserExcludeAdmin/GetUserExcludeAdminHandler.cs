using Application.Features.Auth.Common;
using Application.Features.Common.Interfaces;
using Application.Features.User.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.GetUserExcludeAdmin
{
    internal class GetUserExcludeAdminHandler : IRequestHandler<GetUserExcludeAdminQuery, List<UserDetailsResponse>>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IIdentityService identityService;
        public GetUserExcludeAdminHandler(IAppDbContext appDbContext, IIdentityService identityService)
        {
            this.appDbContext = appDbContext;
            this.identityService = identityService;
        }

        public async Task<List<UserDetailsResponse>> Handle(GetUserExcludeAdminQuery request, CancellationToken cancellationToken)
        {
            var ids = await identityService.GetUserExcludeAdmin();
            var users = await appDbContext.UsersDetails
                    .Where(u => !ids.Contains(u.Id))
                    .ToListAsync(cancellationToken);
            return users.Select(u=>UserMapper.ToResponse(u)).ToList();
        }
    }
}
