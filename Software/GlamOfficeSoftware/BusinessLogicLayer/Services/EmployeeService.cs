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

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByName(string name)
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetEmployeesByName(name);
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByLastName(string surname)
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetEmployeesByLastName(surname);
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByKeyPhrase(string word)
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetEmployeesByKeyPhrase(word);
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByWorkPosition(string workPositionName)
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetEmployeesByWorkPosition(workPositionName);
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByRole(string roleName)
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetEmployeesByRole(roleName);
            }
        }
    }
}
