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
        Task ChangeReservationStatusAndPaymentAsync(int reservationId, ReservationStatuses status, bool isPaid);

        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
    }
}
