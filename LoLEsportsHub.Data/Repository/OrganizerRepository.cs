using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Repository
{
    public class OrganizerRepository : BaseRepository<Organizer, Guid>, IOrganizerRepository
    {
        public OrganizerRepository(LoLEsportsHubDbContext dbContext)
            : base(dbContext)
        {
            
        }
    }
}
