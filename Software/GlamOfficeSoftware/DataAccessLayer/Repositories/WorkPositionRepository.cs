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
    public class WorkPositionRepository: Repository<WorkPosition>
    {

        public async Task<IEnumerable<WorkPosition>> GetAllWorkPositionsAsync()
        {
            return await context.WorkPositions.ToListAsync();
        }
        public async Task<WorkPosition> GetByNameAsync(string name)
        {
            return await context.WorkPositions.FirstOrDefaultAsync(wp => wp.Name == name);
        }
    }
}
