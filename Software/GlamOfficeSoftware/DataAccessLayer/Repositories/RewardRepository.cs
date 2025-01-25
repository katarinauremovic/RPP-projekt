using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RewardRepository : Repository<Reward>
    {
        public async override Task<IEnumerable<Reward>> GetAllAsync()
        {
            return await items.Include(r => r.LoyaltyLevel).ToListAsync();
        }

        public async Task<IEnumerable<Reward>> GetRewardsByLoyaltyLevelName(string loyaltyLevelName)
        {
            return await items.Where(r => r.LoyaltyLevel.Name == loyaltyLevelName).Include(r => r.LoyaltyLevel).ToListAsync();
        }
    }
}
