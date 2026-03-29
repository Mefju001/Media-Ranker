using Application.Common.Interfaces;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ITokenRepository : IRepository<Token>
    {
        Task<int> DeleteTokensFromUserId(Guid userId, string? jti, CancellationToken cancellationToken);
        Task<int> CleanUpTokensAsync(CancellationToken cancellationToken);
        Task<Token> GetByJtiAsync(string jti, CancellationToken cancellationToken);
    }
}
