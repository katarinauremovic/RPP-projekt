using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.DTOs;
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

        public async Task<IEnumerable<ClientDTO>> GetAllClientsDTOAsync()
        {
            using (var repo = new ClientRepository())
            {
                var clients = await repo.GetAllAsync();

                var clientsDTO = clients.Select(c => new ClientDTO
                {
                    Firstname = c.Firstname,
                    Lastname = c.Lastname,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    RewardPointsCount = c.RewardPoints.Count,
                    GiftCardDescription = c.GiftCard?.Description ?? "No GiftCard",
                    ReservationsDates = string.Join(", ", c.Reservations.Select(r => r.Date.ToString().Split(' ')[0])),
                    ReviewsComments = string.Join(", ", c.Reviews.Select(r => r.Comment))
                }).ToList();

                return clientsDTO;
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

        public async Task<IEnumerable<Client>> GetClientsByFirstAndLastNamePattern(string firstAndLastNamePattern)
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetClientsByFirstAndLastNamePattern(firstAndLastNamePattern);
            }
        }

        public async Task<IEnumerable<Client>> GetClientsByEmailPattern(string emailPattern)
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetClientsByEmailPattern(emailPattern);
            }
        }

        public async Task<IEnumerable<Client>> GetClientsByPhoneNumberPattern(string phoneNumberPattern)
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetClientsByPhoneNumberPattern(phoneNumberPattern);
            }
        }
    }
}
