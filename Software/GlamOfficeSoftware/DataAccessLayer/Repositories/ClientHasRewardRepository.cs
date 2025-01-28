using EntityLayer.Entities;
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
    }
}
