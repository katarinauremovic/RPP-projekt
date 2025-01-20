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
        Task<IEnumerable<Receipt>> GetAllReceiptsAsync();
        Task AddNewReceiptAsync(Receipt receipt);
        Task<Receipt> VoidReceiptAsync(int receiptId, bool wantsGiftCardRecover = false);
    }
}
