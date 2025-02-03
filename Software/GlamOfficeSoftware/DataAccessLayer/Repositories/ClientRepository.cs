using DataAccessLayer.Interfaces;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ClientRepository : Repository<Client>
    {
        public async override Task<Client> GetByIdAsync(int id)
        {
            return await items.Include(c => c.Client_has_Reward)
                .Include(c => c.LoyaltyLevel)
                .Include(c => c.GiftCard)
                .Include(c => c.Reservations)
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync(c => c.idClient == id);
        }

        public async override Task<IEnumerable<Client>> GetAllAsync()
        {
            return await items.Include(c => c.Client_has_Reward)
                .Include(c => c.LoyaltyLevel)
                .Include(c => c.GiftCard)
                .Include(c => c.Reservations)
                .Include(c => c.Reviews)
                .ToListAsync();
        }

        public async Task UpdateClientAsync(Client client)
        {
            var clientDb = items.Attach(client);

            if (clientDb != null)
            {
                clientDb.Firstname = client.Firstname;
                clientDb.Lastname = client.Lastname;
                clientDb.Email = client.Email;
                clientDb.PhoneNumber = client.PhoneNumber;
                clientDb.Points = client.Points;
                clientDb.GiftCard_idGiftCard = client.GiftCard_idGiftCard;
                clientDb.GiftCard = client.GiftCard;
                clientDb.LoyaltyLevel_id = client.LoyaltyLevel_id;
                clientDb.LoyaltyLevel = client.LoyaltyLevel;

                await SaveChangesAsync();
            }
        }

        public async Task<bool> IsClientInTheRewardSystemAsync(int clientId)
        {
            return await items
                .Include(c => c.LoyaltyLevel)
                .AnyAsync(c => c.idClient == clientId && c.LoyaltyLevel.Name != LoyaltyLevels.NotInRewardSystem.ToString());
        }

        public async Task<IEnumerable<Client>> GetClientsByFirstAndLastNamePattern(string firstAndLastNamePattern)
        {
            return await items.Where(i => i.Firstname.Contains(firstAndLastNamePattern) || i.Lastname.Contains(firstAndLastNamePattern)).ToArrayAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsByEmailPattern(string emailPattern)
        {
            return await items.Where(i => i.Email.Contains(emailPattern)).ToArrayAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsByPhoneNumberPattern(string phoneNumberPattern)
        {
            return await items.Where(i => i.PhoneNumber.Contains(phoneNumberPattern)).ToArrayAsync();
        }

        public async Task AssignGiftCardToClientAsync(int clientId, int giftCardId)
        {
            var client = await items.FirstOrDefaultAsync(c => c.idClient == clientId);

            if (client != null)
            {
                client.GiftCard_idGiftCard = giftCardId; 
                await SaveChangesAsync();
            }
        }
    }
}
