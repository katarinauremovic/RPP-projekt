using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ClientHasRewardService : IClientHasRewardService
    {
        public async Task<IEnumerable<Client_has_Reward>> GetClientHasRewardsForClientAsync(int clientId)
        {
            using (var repo = new ClientHasRewardRepository())
            {
                return await repo.GetClientHasRewardsForClientAsync(clientId);
            }
        }
    }
}
