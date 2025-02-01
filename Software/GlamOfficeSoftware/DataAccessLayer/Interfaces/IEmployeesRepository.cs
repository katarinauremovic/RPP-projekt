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
        Task<IEnumerable<Employee>> GetEmployeesByName(string name);
        Task<IEnumerable<Employee>> GetEmployeesByLastName(string surname);
        Task<IEnumerable<Employee>> GetEmployeesByKeyPhrase(string word);
        Task<IEnumerable<Employee>> GetEmployeeesByWorkingPosition(int workingPosition);

        Task<IEnumerable<Employee>> GetEmployeesByRole(int role);
    }
}
