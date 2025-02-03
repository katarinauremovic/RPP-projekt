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
        Task<Client> GetClientByIdAsync(int clientId);
        Task<IEnumerable<ClientDTO>> GetAllClientsDTOAsync();
        Task UpdateClientAsync(Client client);
        Task EditClientAsync(ClientDTO client);
        Task AddNewClient(Client client);
        Task RemoveClient(ClientDTO clientDTO);
        Task<IEnumerable<ClientDTO>> GetClientsByFirstAndLastNamePattern(string firstAndLastNamePattern);
        Task<IEnumerable<ClientDTO>> GetClientsByEmailPattern(string emailPattern);
        Task<IEnumerable<ClientDTO>> GetClientsByPhoneNumberPattern(string phoneNumberPattern);
        Task<bool> IsClientInTheRewardSystemAsync(int clientId);
        Task UpdateClientsLoyaltyLevelAsync(int clientId, int loyaltyLevelId);
        Task AddPointsToClientAsync(int clientId, int points);
        Task SubtractPointsFromClientAsync(int clientId, int pointsToSubtract);

        Task AssignGiftCardToClientAsync(int clientId, int giftCardId);


    }
}
