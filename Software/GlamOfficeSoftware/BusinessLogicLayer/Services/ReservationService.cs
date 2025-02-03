using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ReservationService : IReservationService
    {
        public async Task ChangeReservationStatusAndPaymentAsync(int reservationId, ReservationStatuses status, bool isPaid)
        {
            using (var repo = new ReservationRepository())
            {
                var reservation = await repo.GetByIdAsync(reservationId);
                if (reservation != null)
                {
                    reservation.Status = status.ToString();
                    reservation.isPaid = isPaid;
                    await repo.UpdateReservationAsync(reservation);                
                }
            }
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            using (var repo = new ReservationRepository())
            {
                return await repo.GetAllReservationsAsync();
            }
        }

        public async Task<Reservation> GetLastReservationAsync()
        {
            using (var repo = new ReservationRepository())
            {
                return await repo.GetLastReservationAsync();
            }
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            using (var repo = new ReservationRepository())
            {
                await repo.AddAsync(reservation);
            }
        }
    }
}
