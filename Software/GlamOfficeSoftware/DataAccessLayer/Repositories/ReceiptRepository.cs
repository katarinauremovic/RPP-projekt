using DataAccessLayer.Interfaces;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ReceiptRepository : Repository<Receipt>
    {
        public async override Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await items.Include(r => r.Reservation).ToListAsync();
        }

        public async Task<Receipt> GetLastReceiptAsync()
        {
            return await items
                .Include(r => r.Reservation)
                .Include(r => r.Reservation.Client)
                .OrderByDescending(r => r.idReceipt)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetGiftCardIdByReceiptAsync(Receipt receipt)
        {
            return await items.Where(r => r.idReceipt == receipt.idReceipt)
                .Select(r => r.Reservation.Client.GiftCard_idGiftCard.Value)
                .FirstOrDefaultAsync();
        }

        public async Task ChangeReceiptStatusAsync(Receipt receipt)
        {
            var receiptDb = await items.FirstOrDefaultAsync(r => r.idReceipt == receipt.idReceipt);
            receiptDb.Status = receipt.Status;
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByReceiptNumberPattrern(string receiptNumber)
        {
            return await items.Where(r => r.ReceiptNumber.Contains(receiptNumber)).ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByClientsFirstAndLastNamePattern(string firstAndLastNamePattern)
        {
            return await items
                .Where(r => r.Reservation.Client.Firstname.Contains(firstAndLastNamePattern) ||
                            r.Reservation.Client.Lastname.Contains(firstAndLastNamePattern))
                .ToListAsync();
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByEmployeesFirstAndLastNamePattern(string firstAndLastNamePattern)
        {
            return await items
                .Where(r => r.Reservation.Employee.Firstname.Contains(firstAndLastNamePattern) ||
                            r.Reservation.Employee.Lastname.Contains(firstAndLastNamePattern))
                .ToListAsync();
        }
    }
}
