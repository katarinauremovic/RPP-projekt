using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class LoyaltyLevelRepository : Repository<LoyaltyLevel>
    {
        public async Task<LoyaltyLevel> GetLoyaltyLevelByNameAsync(string name)
        {
            return await items.FirstOrDefaultAsync(l => l.Name == name);
        }
    }
}
