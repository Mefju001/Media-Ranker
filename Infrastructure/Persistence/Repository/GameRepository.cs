using Application.Common.Interfaces;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class GameRepository:IGameRepository
    {
        private readonly AppDbContext _context;
        public GameRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddListOfGames(List<GameDomain>games)
        {
            await _context.Games.AddRangeAsync(games); 
        }
    }
}
