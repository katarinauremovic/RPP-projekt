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

        public override async Task<Reward> GetByIdAsync(int id)
        {
            return await items.Include(r => r.LoyaltyLevel).FirstOrDefaultAsync(r => r.idReward == id);
        }

        public async Task<IEnumerable<Reward>> GetRewardsByLoyaltyLevelNameAsync(string loyaltyLevelName)
        {
            return await items.Where(r => r.LoyaltyLevel.Name == loyaltyLevelName).Include(r => r.LoyaltyLevel).ToListAsync();
        }

        public async Task<IEnumerable<Reward>> GetRewardsWithinClientsPointsAsync(int points)
        {
            return await items.Where(r => r.LoyaltyLevel.RequiredPoints <= points).Include(r => r.LoyaltyLevel).ToListAsync();
        }
    }
}
