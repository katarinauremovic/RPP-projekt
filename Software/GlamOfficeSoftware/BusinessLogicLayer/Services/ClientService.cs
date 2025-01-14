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
    public class ClientService : IClientService
    {
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetAllAsync();
            }
        }

        public async Task<Client> GetByEmailAsync(string email)
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetByEmailAsync(email);
            }
        }

        public async Task<IEnumerable<Client>> GetWithActiveGiftCardsAsync()
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetWithActiveGiftCardsAsync();
            }
        }

        public async Task<IEnumerable<Client>> GetWithDetailsAsync()
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetWithDetailsAsync();
            }
        }

        public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber)
        {
            using (var repo = new ClientRepository())
            {
                return await repo.ExistsByPhoneNumberAsync(phoneNumber);
            }
        }

        public async Task<IEnumerable<Client>> GetClientsBySpendingAsync(decimal topN)
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetClientsBySpendingAsync(topN);
            }
        }

        public async Task<IEnumerable<Client>> GetClientsWithReservationsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetClientsWithReservationsInDateRangeAsync(startDate, endDate);
            }
        }

        public async Task<IEnumerable<Client>> GetClientsWithoutReservationsAsync()
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetClientsWithoutReservationsAsync();
            }
        }

        public async Task<IEnumerable<Client>> GetClientsWithExpiredGiftCardsAsync()
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetClientsWithExpiredGiftCardsAsync();
            }
        }

        public async Task<IEnumerable<Client>> GetClientsByRewardTypeAsync(string rewardType)
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetClientsByRewardTypeAsync(rewardType);
            }
        }
    }
}
