using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ReservationRepository : Repository<Reservation>
    {
        public override Task<Reservation> GetByIdAsync(int id)
        {
            return items
                .Include(r => r.Client)
                .Include(r => r.Day)
                .Include(r => r.Employee)
                .Include(r => r.Reservation_has_Treatment)
                .Where(r => r.idReservation == id)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            var reservationDb = await items.FirstOrDefaultAsync(r => r.idReservation == reservation.idReservation);

            reservationDb = reservation;

            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await context.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetLastReservationAsync()
        {
            return await context.Reservations.OrderByDescending(r => r.idReservation).FirstOrDefaultAsync();
        }
    }
}
