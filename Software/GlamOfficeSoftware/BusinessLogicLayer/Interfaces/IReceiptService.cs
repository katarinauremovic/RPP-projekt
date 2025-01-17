using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IReceiptService
    {
        Task<Receipt> GenerateReceiptAsync(int reservationId, bool applyDiscount = false);
        Task<IEnumerable<Receipt>> GetReceiptsByClientAsync(int clientId);
        Task<IEnumerable<Receipt>> GetReceiptsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task ApplyDiscountAsync(int receiptId, decimal discountAmount);
        Task<decimal> GetTotalRevenueAsync();
    }
}
