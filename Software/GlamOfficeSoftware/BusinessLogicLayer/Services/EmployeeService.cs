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

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByNameAsync(string name)
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetEmployeesByNameAsync(name);
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByLastNameAsync(string surname)
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetEmployeesByLastNameAsync(surname);
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByKeyPhraseAsync(string word)
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetEmployeesByKeyPhraseAsync(word);
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByWorkPositionAsync(string workPositionName)
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetEmployeesByWorkPositionAsync(workPositionName);
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByRoleAsync(string roleName)
        {
            using (var repo = new EmployeesRepository())
            {
                return await repo.GetEmployeesByRoleAsync(roleName);
            }
        }



        public async Task AddNewEmployeeAsync(Employee employee)
        {
            using (var repo = new EmployeesRepository())
            {
                await repo.AddAsync(employee);
            }
        }

        public async Task<IEnumerable<WorkPosition>> GetWorkPositionsAsync()
        {
            using (var repo = new WorkPositionRepository())
            {
                return await repo.GetWorkPositionsAsync();
            }
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            using (var repo = new RolesRepository())
            {
                return await repo.GetRolesAsync();
            }
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            using (var repo = new EmployeesRepository())
            {
                var existingUser = await repo.GetEmployeeByUsernameAsync(username);
                return existingUser != null;
            }
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            using (var repo = new EmployeesRepository())
            {
                await repo.DeleteEmployeeAsync(employeeId);
            }
        }
    }
}
