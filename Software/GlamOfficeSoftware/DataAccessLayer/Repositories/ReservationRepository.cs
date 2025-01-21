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
    }
}
