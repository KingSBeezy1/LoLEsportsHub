using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository;
using LoLEsportsHub.Data.Repository.Interfaces;
using LoLEsportsHub.Services.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoLEsportsHub.Services.Core
{
    public class OrganizerService : IOrganizerService
    {
        private readonly IOrganizerRepository _organizerRepository;

        public OrganizerService(IOrganizerRepository organizerRepository)
        {
                this._organizerRepository = organizerRepository;
        }

        public async Task<bool> ExistsByIdAsync(string? id)
        {
            bool result = false;
            if (!String.IsNullOrWhiteSpace(id))
            {
                result = await this._organizerRepository
                    .GetAllAttached()
                    .AnyAsync(o => o.Id.ToString().ToLower() == id.ToLower());
            }

            return result;
        }

        public async Task<bool> ExistsByUserIdAsync(string? userId)
        {
            bool result = false;
            if (!String.IsNullOrWhiteSpace(userId))
            {
                result = await this._organizerRepository
                    .GetAllAttached()
                    .AnyAsync(o => o.UserId.ToLower() == userId.ToLower());
            }
            return result;
        }

        public async Task<Guid?> GetIdByUserIdAsync(string? userId)
        {
            Guid? organizerId = null;
            if (!String.IsNullOrWhiteSpace(userId))
            {
                Organizer? organizer = await this._organizerRepository
                    .FirstOrDefaultAsync(m => m.UserId.ToLower() == userId.ToLower());

                if (organizer != null)
                {
                    organizerId = organizer.Id;
                } 
            }
            return organizerId;
        }
    }
}
