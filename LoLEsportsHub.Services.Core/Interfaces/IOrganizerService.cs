using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Services.Core.Interfaces
{
    public interface IOrganizerService
    {
        Task<Guid?> GetIdByUserIdAsync(string? userId);

        Task<bool> ExistsByIdAsync(string? id);

        Task<bool> ExistsByUserIdAsync(string? userId);
    }
}
