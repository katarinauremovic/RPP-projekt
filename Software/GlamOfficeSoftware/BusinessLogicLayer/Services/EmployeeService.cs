using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetAllEmployeesAsync();
            }
        }
    }
}
