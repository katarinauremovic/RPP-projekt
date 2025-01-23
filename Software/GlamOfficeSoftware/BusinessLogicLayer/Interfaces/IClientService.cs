using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<IEnumerable<ClientDTO>> GetAllClientsDTOAsync();
        Task UpdateClientAsync(ClientDTO client);
        Task AddNewClient(Client client);
        Task RemoveClient(ClientDTO clientDTO);
        Task<IEnumerable<ClientDTO>> GetClientsByFirstAndLastNamePattern(string firstAndLastNamePattern);
        Task<IEnumerable<ClientDTO>> GetClientsByEmailPattern(string emailPattern);
        Task<IEnumerable<ClientDTO>> GetClientsByPhoneNumberPattern(string phoneNumberPattern);
        Task<Client> GetByEmailAsync(string email);
        Task<IEnumerable<Client>> GetWithActiveGiftCardsAsync();
        Task<IEnumerable<Client>> GetWithDetailsAsync();
        Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);
        Task<IEnumerable<Client>> GetClientsBySpendingAsync(decimal topN);
        Task<IEnumerable<Client>> GetClientsWithReservationsInDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Client>> GetClientsWithoutReservationsAsync();
        Task<IEnumerable<Client>> GetClientsWithExpiredGiftCardsAsync();
        Task<IEnumerable<Client>> GetClientsByRewardTypeAsync(string rewardType);
    }
}
