using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RolesRepository : Repository<Role>
    {
        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await context.Roles.ToListAsync();
        }
    }
}
