using LoLEsportsHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Repository.Interfaces
{
    public interface IOrganizerRepository
        : IRepository<Organizer, Guid>, IAsyncRepository<Organizer, Guid>
    {

    }
}
