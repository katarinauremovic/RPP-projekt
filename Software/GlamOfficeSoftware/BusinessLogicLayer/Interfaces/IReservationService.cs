using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int reservationId);
        Task AddNewReservationAsync(Reservation reservation);
        Task UpdateReservationAsync(Reservation reservation);
        Task ChangeReservationStatusAndPaymentAsync(int reservationId, ReservationStatuses status, bool isPaid);
    }
}
