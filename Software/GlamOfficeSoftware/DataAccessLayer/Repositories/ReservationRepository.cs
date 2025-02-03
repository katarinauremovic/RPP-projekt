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
