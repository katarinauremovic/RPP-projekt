using DataAccessLayer.Interfaces;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ReceiptRepository : Repository<Receipt>, IReceiptRepository
    {
        public async override Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await items.Include(r => r.Reservation).ToListAsync();
        }

        public async Task ApplyDiscountAsync(int receiptId, decimal discountAmount)
        {
            var receipt = await GetReceiptByIdAsync(receiptId);
            receipt.Balance -= discountAmount;
            await SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int receiptId)
        {
            return await items.AnyAsync(r => r.idReceipt == receiptId);
        }

        public async Task<Receipt> GenerateReceiptAsync(int reservationId)
        {
            var reservation = await context.Reservations
                .Include(r => r.Reservation_has_Treatment.Select(rt => rt.Treatment))
                .FirstOrDefaultAsync(r => r.idReservation == reservationId);

            if (reservation == null)
                return null;

            // Izračun ukupnog iznosa za tretmane u rezervaciji
            var totalBalance = reservation.Reservation_has_Treatment
                .Sum(rt => rt.Treatment.Price * (rt.Amount ?? 1));

            var receipt = new Receipt
            {
                ReceiptNumber = GenerateReceiptNumber(),
                Balance = (decimal)totalBalance,
                Reservation_idReservation = reservationId
            };

            items.Add(receipt);
            await SaveChangesAsync();

            return receipt;
        }

        public string GenerateReceiptNumber()
        {
            return Guid.NewGuid().ToString();
        }

        public string GenerateReceiptNumberAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Receipt> GetReceiptByIdAsync(int receiptId)
        {
            return await items.FindAsync(receiptId);
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByClientAsync(int clientId)
        {
            return await items
                .Where(r => r.Reservation.Client_idClient == clientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await items
               .Where(r => r.Reservation.Date >= startDate && r.Reservation.Date <= endDate)
               .ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByEmployeeAsync(int employeeId)
        {
            return await items
                .Where(r => r.Reservation.Employee_idEmployee == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByTreatmentAsync(int treatmentId)
        {
            return await items
                .Where(r => r.Reservation.Reservation_has_Treatment
                    .Any(rt => rt.Treatment_idTreatment == treatmentId))
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await items.SumAsync(r => r.Balance ?? 0);
        }
    }
}
