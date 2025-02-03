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

        public async Task AddClientHasRewardAsync(Client_has_Reward clientHasReward)
        {
            using (var repo = new ClientHasRewardRepository())
            {
                await repo.AddAsync(clientHasReward);
            }
        }

        public async Task<Client_has_Reward> GetRewardByRedeemCode(string code)
        {
            using (var repo = new ClientHasRewardRepository())
            {
                var reward = await repo.GetRewardByRedeemCode(code);
                Console.WriteLine(reward.Reward_idReward);
                return reward;
            }
        }

        public async Task UpdateClientHasReward(Client_has_Reward chr)
        {
            using (var repo = new ClientHasRewardRepository())
            {
                chr.Status = ClientHasRewardStatuses.Redeemed.ToString();
                chr.RedeemDate = DateTime.Now;
                await repo.UpdateClientHasReward(chr);
            }
        }
    }
}
