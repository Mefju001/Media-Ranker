using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMediaRepository
    {
       Task<WebApplication1.Domain.Entities.Media> GetMediaById(int mediaId);
    }
}
