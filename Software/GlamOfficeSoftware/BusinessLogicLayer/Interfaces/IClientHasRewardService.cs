using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IClientHasRewardService
    {
        Task<IEnumerable<Client_has_Reward>> GetClientHasRewardsForClientAsync(int clientId);
        Task AddClientHasRewardAsync(Client_has_Reward clientHasReward);
    }
}
