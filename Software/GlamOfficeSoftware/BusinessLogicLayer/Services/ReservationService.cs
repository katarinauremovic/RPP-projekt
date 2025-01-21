﻿using BusinessLogicLayer.Interfaces;
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
        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            using (var repo = new ReservationRepository())
            {
                return await repo.GetAllAsync();
            }
        }

        public async Task AddNewReservationAsync(Reservation reservation)
        {
            using (var repo = new ReservationRepository())
            {
                await repo.AddAsync(reservation);
            }
        }

        public async Task<Reservation> UpdateReservationAsync(Reservation reservation)
        {
            using (var repo = new ReservationRepository())
            {
                await repo.UpdateReservationAsync(reservation);
                return reservation;
            }
        }

        public async Task ChangeReservationStatus(int reservationId, ReservationStatuses status)
        {
            using (var repo = new ReservationRepository())
            {
                var reservation = await repo.GetByIdAsync(reservationId);
                if (reservation != null)
                {
                    reservation.Status = status.ToString();
                    await repo.UpdateReservationAsync(reservation);                
                }
            }
        }
    }
}
