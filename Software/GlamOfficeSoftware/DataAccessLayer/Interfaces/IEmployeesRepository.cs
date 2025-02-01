using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IEmployeesRepository : IRepository<Employee>
    {
        Task UpdateEmployeeAsync(Employee employee);

        Task <IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
        Task<IEnumerable<EmployeeDTO>> GetEmployeesByName(string name);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByLastName(string surname);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByKeyPhrase(string word);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByWorkPosition(string workPositionName);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByRole(string roleName);
        
    }
}
