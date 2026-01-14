using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Infrastructure.Persistence;

namespace Infrastructure.Persistence.Repository
{
    public class MediaRepository: IMediaRepository
    {
        private readonly AppDbContext appDbContext;

        public MediaRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<WebApplication1.Domain.Entities.Media> GetMediaById(int mediaId)
        {
            var media = await appDbContext.Medias
                .Where(m => m.Id == mediaId)
                .FirstOrDefaultAsync();
            if (media == null) throw new Exception("Media not found");
            return media;
        }
    }
}
