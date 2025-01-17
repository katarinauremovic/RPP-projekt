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
    public class ReceiptRepository : Repository<Receipt>, IReceiptRepository
    {
        public async override Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await items.Include(r => r.Reservation).ToListAsync();
        }
    }
}
