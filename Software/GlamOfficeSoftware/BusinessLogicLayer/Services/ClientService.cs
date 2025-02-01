using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

        public async Task<Client> GetClientByIdAsync(int clientId)
        {
            using (var repo = new ClientRepository())
            {
                return await repo.GetByIdAsync(clientId);
            }
        }

        public async Task<IEnumerable<ClientDTO>> GetAllClientsDTOAsync()
        {
            using (var repo = new ClientRepository())
            {
                var clients = await repo.GetAllAsync();
                var clientsDTO = clients.Select(ConvertClientToClientDTO).ToList();
                return clientsDTO;
            }
        }

        public async Task UpdateClientAsync(ClientDTO clientDTO)
        {
            using (var repo = new ClientRepository())
            {
                var client = await repo.GetByIdAsync(clientDTO.Id);

                if (client == null)
                {
                    throw new ClientNotFoundException($"Client with ID {clientDTO.Id} does not exist.");
                }

                client.Firstname = clientDTO.Firstname;
                client.Lastname = clientDTO.Lastname;
                client.Email = clientDTO.Email;
                client.PhoneNumber = clientDTO.PhoneNumber;

                await repo.UpdateClientAsync(client);         
            }
        }

        public ClientDTO ConvertClientToClientDTO(Client client)
        {
            return new ClientDTO
            {
                Id = client.idClient,
                Firstname = client.Firstname,
                Lastname = client.Lastname,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Points = client.Points.Value,
                LoyaltyLevel = client.LoyaltyLevel.Name,
                GiftCardDescription = client.GiftCard?.Description ?? "No GiftCard",
                ReservationsDates = client.Reservations.Any()
                    ? string.Join(", ", client.Reservations.Select(r => r.Date.ToString().Split(' ')[0]))
                    : "No reservations",
                ReviewsComments = client.Reviews.Any()
                    ? string.Join(", ", client.Reviews.Select(r => r.Comment))
                    : "No reviews"
            };
        }

        public async Task AddNewClient(Client client)
        {
            using (var repo = new ClientRepository())
            {
                await repo.AddAsync(client);
            }
        }

        public async Task RemoveClient(ClientDTO clientDTO)
        {
            using (var repo = new ClientRepository())
            {
                var client = await repo.GetByIdAsync(clientDTO.Id);

                await repo.RemoveAsync(client);
            }
        }

        public async Task<IEnumerable<ClientDTO>> GetClientsByFirstAndLastNamePattern(string firstAndLastNamePattern)
        {
            using (var repo = new ClientRepository())
            {
                var clients = await repo.GetClientsByFirstAndLastNamePattern(firstAndLastNamePattern);
                var clientsDto = clients.Select(ConvertClientToClientDTO).ToList();
                return clientsDto;
            }
        }

        public async Task<IEnumerable<ClientDTO>> GetClientsByEmailPattern(string emailPattern)
        {
            using (var repo = new ClientRepository())
            {
                var clients = await repo.GetClientsByEmailPattern(emailPattern);
                var clientsDto = clients.Select(ConvertClientToClientDTO).ToList();
                return clientsDto;
            }
        }

        public async Task<IEnumerable<ClientDTO>> GetClientsByPhoneNumberPattern(string phoneNumberPattern)
        {
            using (var repo = new ClientRepository())
            {
                var clients = await repo.GetClientsByPhoneNumberPattern(phoneNumberPattern);
                var clientsDto = clients.Select(ConvertClientToClientDTO).ToList();
                return clientsDto;
            }
        }

        public async Task<bool> IsClientInTheRewardSystemAsync(int clientId)
        {
            using (var repo = new ClientRepository())
            {
                return await repo.IsClientInTheRewardSystemAsync(clientId);
            }
        }

        public async Task AddClientToRewardSystemAsync(int clientId)
        {
            using (var repo = new ClientRepository())
            {
                var client = await repo.GetByIdAsync(clientId);
                client.Points = 200;

                var rewardSystem = new RewardSystem();
                await rewardSystem.UpdateClientsLoyaltyLevelAsync(client);

                await repo.UpdateClientAsync(client);
            }
        }

        public async Task UpdateClientsLoyaltyLevelAsync(int clientId, int loyaltyLevelId)
        {
            using (var repo = new ClientRepository())
            {
                var client = await repo.GetByIdAsync(clientId);
                client.LoyaltyLevel_id = loyaltyLevelId;
                await repo.UpdateClientAsync(client);
            }
        }

        public async Task AddPointsToClientAsync(int clientId, int pointsToAdd)
        {
            using (var repo = new ClientRepository())
            {
                var client = await repo.GetByIdAsync(clientId);
                client.Points = client.Points + pointsToAdd;

                var rewardSystem = new RewardSystem();
                client.LoyaltyLevel_id = await rewardSystem.UpdateClientsLoyaltyLevelAsync(client);

                await repo.UpdateClientAsync(client);
            }
        }

        public async Task SubtractPointsFromClientAsync(int clientId, int pointsToSubtract)
        {
            if (pointsToSubtract <= 0)
            {
                throw new ArgumentException("Substract number must be more then 0.");
            }

            using (var repo = new ClientRepository())
            {
                var client = await repo.GetByIdAsync(clientId);
                client.Points = client.Points - pointsToSubtract;

                var rewardSystem = new RewardSystem();
                client.LoyaltyLevel_id = await rewardSystem.UpdateClientsLoyaltyLevelAsync(client);

                await repo.UpdateClientAsync(client);
            }
        }
    }
}
