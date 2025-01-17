using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IReceiptRepository
    {
        string GenerateReceiptNumber();
        Task<bool> IsClientsGiftCard(int giftCardId, string promoCode);
        Task<IEnumerable<Receipt>> GetReceiptsByClientAsync(int clientId);
        Task<bool> ExistsAsync(int receiptId);
        Task<Receipt> GenerateReceiptAsync(int reservationId);
        Task<IEnumerable<Receipt>> GetReceiptsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task ApplyDiscountAsync(int receiptId, decimal discountAmount);
        Task<Receipt> GetReceiptByIdAsync(int receiptId);
        Task<IEnumerable<Receipt>> GetReceiptsByTreatmentAsync(int treatmentId);
        Task<decimal> GetTotalRevenueAsync();
        Task<IEnumerable<Receipt>> GetReceiptsByEmployeeAsync(int employeeId);
    }
}
