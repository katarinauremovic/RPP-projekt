using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ClientService : Service<Client>, IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository) : base(clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> GetByEmailAsync(string email)
        {
            return await _clientRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<Client>> GetWithActiveGiftCardsAsync()
        {
            return await _clientRepository.GetWithActiveGiftCardsAsync();
        }

        public async Task<IEnumerable<Client>> GetWithDetailsAsync()
        {
            return await _clientRepository.GetWithDetailsAsync();
        }

        public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber)
        {
            return await _clientRepository.ExistsByPhoneNumberAsync(phoneNumber);
        }

        public async Task<IEnumerable<Client>> GetClientsBySpendingAsync(decimal topN)
        {
            return await _clientRepository.GetClientsBySpendingAsync(topN);
        }

        public async Task<IEnumerable<Client>> GetClientsWithReservationsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _clientRepository.GetClientsWithReservationsInDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<Client>> GetClientsWithoutReservationsAsync()
        {
            return await _clientRepository.GetClientsWithoutReservationsAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsWithExpiredGiftCardsAsync()
        {
            return await _clientRepository.GetClientsWithExpiredGiftCardsAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsByRewardTypeAsync(string rewardType)
        {
            return await _clientRepository.GetClientsByRewardTypeAsync(rewardType);
        }
    }
}
