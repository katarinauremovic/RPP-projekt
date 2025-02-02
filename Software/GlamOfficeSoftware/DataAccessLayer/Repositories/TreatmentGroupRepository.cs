using DataAccessLayer.Interfaces;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class TreatmentGroupRepository:Repository<TreatmentGroup>
    {
        public async Task<IEnumerable<TreatmentGroup>> GetAllTreatmentGroupsAsync()
        {
            return await context.TreatmentGroups.ToListAsync();
        }
        public async Task<TreatmentGroup> GetByNameAsync(string name)
        {
            return await context.TreatmentGroups.FirstOrDefaultAsync(g => g.Name == name);
        }
    }
}
