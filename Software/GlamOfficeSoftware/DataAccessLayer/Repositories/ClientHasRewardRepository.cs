using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ClientHasRewardRepository : Repository<Client_has_Reward>
    {
        public async Task<IEnumerable<Client_has_Reward>> GetClientHasRewardsForClientAsync(int clientId)
        {
            return await items.Where(chr => chr.Client_idClient == clientId).ToListAsync();
        }

        public async Task<Client_has_Reward> GetRewardByRedeemCode(string code)
        {
            return await items
                .FirstOrDefaultAsync(chr => chr.ReedemCode == code &&
                chr.Status == ClientHasRewardStatuses.Active.ToString());
        }

        public async Task UpdateClientHasReward(Client_has_Reward chr)
        {
            var chrDb = await items.
                Where(chrr => chr.Client_idClient == chr.Client_idClient &&
                chrr.Reward_idReward == chr.Reward_idReward)
                .FirstOrDefaultAsync();

            chrDb = chr;

            await SaveChangesAsync();
        }
    }


}
