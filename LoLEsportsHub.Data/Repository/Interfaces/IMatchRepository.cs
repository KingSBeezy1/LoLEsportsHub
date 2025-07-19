using LoLEsportsHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Repository.Interfaces
{
    public interface IMatchRepository
        : IRepository<Match, Guid>, IAsyncRepository<Match, Guid>
    {

    }
}
