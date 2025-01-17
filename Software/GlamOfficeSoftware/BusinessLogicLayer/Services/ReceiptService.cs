using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ReceiptService : IReceiptService
    {
        public async Task ApplyDiscountAsync(int receiptId, decimal discountAmount)
        {
            using (var repo = new ReceiptRepository())
            {
                await repo.ApplyDiscountAsync(receiptId, discountAmount);
            }
        }

        public async Task<Receipt> GenerateReceiptAsync(int reservationId, bool applyDiscount = false)
        {
            using (var repo = new ReceiptRepository())
            {
                var receipt = await repo.GenerateReceiptAsync(reservationId);
                if (applyDiscount)
                {
                    decimal discountAmount = 10.0m; // Ovo možete zamijeniti s stvarnim iznosom popusta.
                    await repo.ApplyDiscountAsync(receipt.idReceipt, discountAmount);
                }
                return receipt;
            }
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByClientAsync(int clientId)
        {
            using (var repo = new ReceiptRepository())
            {
                return await repo.GetReceiptsByClientAsync(clientId);
            }
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using (var repo = new ReceiptRepository())
            {
                return await repo.GetReceiptsByDateRangeAsync(startDate, endDate);
            }
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            using (var repo = new ReceiptRepository())
            {
                return await repo.GetTotalRevenueAsync();
            }
        }
    }
}
