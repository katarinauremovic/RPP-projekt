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
    public class TreatmentRepository : Repository<Treatment>, ITreatmentRepository
    {
       
        public async Task<string> GetTreatmentGroupNameByIdAsync(int treatmentId)
        {
            var result = await (from t in items
                                join tg in context.TreatmentGroups
                                on t.TreatmentGroup_idTreatmentGroup equals tg.idTreatmentGroup
                                where t.idTreatment == treatmentId
                                select tg.Name)
                                .FirstOrDefaultAsync();

            return result ?? "No Group"; 
        }

        public async Task<IEnumerable<Treatment>> GetTreatmentsByNamePattern(string namePattern)
        {
            return await items
                .Where(t => t.Name.Contains(namePattern)) 
                .ToListAsync();
        }


    }
}
