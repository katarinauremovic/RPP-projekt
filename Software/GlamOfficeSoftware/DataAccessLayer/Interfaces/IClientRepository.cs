using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetByEmailAsync(string email);
        Task<IEnumerable<Client>> GetWithActiveGiftCardsAsync();
        Task<IEnumerable<Client>> GetWithDetailsAsync();
        Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);
        Task<IEnumerable<Client>> GetClientsBySpendingAsync(decimal topN);
        Task<IEnumerable<Client>> GetClientsWithReservationsInDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Client>> GetClientsWithoutReservationsAsync();
        Task<IEnumerable<Client>> GetClientsWithExpiredGiftCardsAsync();
        Task<IEnumerable<Client>> GetClientsByRewardTypeAsync(string rewardType);
        Task<IEnumerable<Client>> GetClientsByFirstAndLastNamePattern(string firstAndLastNamePattern);
        Task<IEnumerable<Client>> GetClientsByEmailPattern(string emailPattern);
        Task<IEnumerable<Client>> GetClientsByPhoneNumberPattern(string phoneNumberPattern);
    }
}
