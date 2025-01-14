using DataAccessLayer.Interfaces;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        // Dohvati klijenta prema email adresi
        public async Task<Client> GetByEmailAsync(string email)
        {
            return await items.FirstOrDefaultAsync(client => client.Email == email);
        }

        // Dohvati klijente koji imaju aktivne poklon kartice
        public async Task<IEnumerable<Client>> GetWithActiveGiftCardsAsync()
        {
            return await items.Where(client => client.GiftCard_idGiftCard != null &&
                                               client.GiftCard.Status == "Aktivno")
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
                                               client.GiftCard.Status == "Isteklo")
                              .Include(client => client.GiftCard)
                              .ToListAsync();
        }

        // Dohvati klijente na temelju vrste nagrade
        public async Task<IEnumerable<Client>> GetClientsByRewardTypeAsync(string rewardType)
        {
            return await items.Include(client => client.RewardPoints.Select(rp => rp.Reward))
                              .Where(client => client.RewardPoints.Any(rp => rp.Reward.Name == rewardType))
                              .ToListAsync();
        }
    }
}
