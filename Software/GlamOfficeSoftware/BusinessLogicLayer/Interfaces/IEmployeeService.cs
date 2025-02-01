using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByName(string name);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByLastName(string surname);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByKeyPhrase(string word);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByWorkPosition(string workPositionName);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByRole(string roleName);
    }
}
