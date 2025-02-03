using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ReservationHasTreatmentService : IReservationHasTreatmentService
    {
        public async Task AddReservationHasTreatmentAsync(Reservation_has_Treatment rht)
        {
            using(var repo = new ReservationHasTreatmentRepository())
            {
                await repo.AddAsync(rht);
            }
        }
    }
}
