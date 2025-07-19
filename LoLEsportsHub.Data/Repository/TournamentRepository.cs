using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Repository
{
    public class TournamentRepository : BaseRepository<Tournament, Guid>, ITournamentRepository
    {
        public TournamentRepository(LoLEsportsHubDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
