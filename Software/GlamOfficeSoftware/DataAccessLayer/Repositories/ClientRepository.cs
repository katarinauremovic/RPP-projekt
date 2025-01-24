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
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        // Dohvaca sve klijente s joinanim podacima
        public async override Task<IEnumerable<Client>> GetAllAsync()
        {
            return await items.Include(c => c.Client_has_Reward)
                .Include(c => c.LoyaltyLevel)
                .Include(c => c.GiftCard)
                .Include(c => c.Reservations)
                .Include(c => c.Reviews)
                .ToListAsync();
        }

        // Azurira klijenta
        public async Task UpdateClientAsync(Client client)
        {
            var clientDb = await items.FirstOrDefaultAsync(c => c.idClient == client.idClient);

            if (clientDb != null)
            {
                clientDb.Firstname = client.Firstname;
                clientDb.Lastname = client.Lastname;
                clientDb.Email = client.Email;
                clientDb.PhoneNumber = client.PhoneNumber;

                await SaveChangesAsync();
            }
        }

        // Dohvati klijenta prema email adresi
        public async Task<Client> GetByEmailAsync(string email)
        {
            return await items.FirstOrDefaultAsync(client => client.Email == email);
        }

        // Dohvati klijente koji imaju aktivne poklon kartice
        public async Task<IEnumerable<Client>> GetWithActiveGiftCardsAsync()
        {
            return await items.Where(client => client.GiftCard_idGiftCard != null &&
                                               client.GiftCard.Status == GiftCardStatuses.Active.ToString())
                              .Include(client => client.GiftCard)
                              .ToListAsync();
        }

        // Dohvati klijente s detaljima (poklon kartice)
        public async Task<IEnumerable<Client>> GetWithDetailsAsync()
        {
            return await items.Include(client => client.GiftCard).ToListAsync();
        }

        // Provjeri postoji li klijent s određenim brojem telefona
        public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber)
        {
            return await items.AnyAsync(client => client.PhoneNumber == phoneNumber);
        }

        // Dohvati klijente s najvećom ukupnom potrošnjom (pretpostavka: potrošnja je zbroj svih rezervacija)
        public async Task<IEnumerable<Client>> GetClientsBySpendingAsync(decimal topN)
        {
            var clients = await items
                .Include(client => client.Reservations)
                .Include(client => client.Reservations.Select(reservation => reservation.Reservation_has_Treatment))
                .Include(client => client.Reservations.Select(reservation => reservation.Reservation_has_Treatment.Select(rht => rht.Treatment)))
                .OrderByDescending(client => client.Reservations
                    .Sum(reservation => reservation.Reservation_has_Treatment
                        .Sum(rht => (rht.Treatment.Price ?? 0) * (rht.Amount ?? 1))))
                .Take((int)topN)
                .ToListAsync();

            return clients;
        }


        // Dohvati klijente s rezervacijama unutar određenog vremenskog razdoblja
        public async Task<IEnumerable<Client>> GetClientsWithReservationsInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await items.Include(client => client.Reservations)
                              .Where(client => client.Reservations.Any(reservation => reservation.Date >= startDate && reservation.Date <= endDate))
                              .ToListAsync();
        }

        // Dohvati klijente bez ikakvih rezervacija
        public async Task<IEnumerable<Client>> GetClientsWithoutReservationsAsync()
        {
            return await items.Where(client => !client.Reservations.Any()).ToListAsync();
        }

        // Dohvati klijente s isteklim poklon karticama
        public async Task<IEnumerable<Client>> GetClientsWithExpiredGiftCardsAsync()
        {
            return await items.Where(client => client.GiftCard_idGiftCard != null &&
                                               client.GiftCard.Status == GiftCardStatuses.Expired.ToString())
                              .Include(client => client.GiftCard)
                              .ToListAsync();
        }

        // Dohvati klijente na temelju vrste nagrade
        public async Task<IEnumerable<Client>> GetClientsByRewardTypeAsync(string rewardType)
        {
            return await items.Include(client => client.Client_has_Reward.Select(rp => rp.Reward))
                              .Where(client => client.Client_has_Reward.Any(rp => rp.Reward.Name == rewardType))
                              .ToListAsync();
        }

        //Dohvati klijente po uzorku imena i prezimena
        public async Task<IEnumerable<Client>> GetClientsByFirstAndLastNamePattern(string firstAndLastNamePattern)
        {
            return await items.Where(i => i.Firstname.Contains(firstAndLastNamePattern) || i.Lastname.Contains(firstAndLastNamePattern)).ToArrayAsync();
        }

        //Dohvati klijente po uzorku na email
        public async Task<IEnumerable<Client>> GetClientsByEmailPattern(string emailPattern)
        {
            return await items.Where(i => i.Email.Contains(emailPattern)).ToArrayAsync();
        }

        //Dohvati klijente po uzorku na email
        public async Task<IEnumerable<Client>> GetClientsByPhoneNumberPattern(string phoneNumberPattern)
        {
            return await items.Where(i => i.PhoneNumber.Contains(phoneNumberPattern)).ToArrayAsync();
        }
    }
}
