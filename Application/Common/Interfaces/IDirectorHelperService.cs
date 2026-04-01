using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Domain.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IDirectorHelperService
    {
        Task<Director> GetOrCreateDirectorAsync(DirectorRequest directorRequest, CancellationToken cancellationToken);
        Task<Dictionary<(string, string), Director>> EnsureDirectorsExistAsync(List<DirectorRequest> directors, CancellationToken cancellationToken);
    }
}
