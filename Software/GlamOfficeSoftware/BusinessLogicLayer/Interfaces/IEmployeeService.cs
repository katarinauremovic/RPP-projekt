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

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByNameAsync(string name);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByLastNameAsync(string surname);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByKeyPhraseAsync(string word);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByWorkPositionAsync(string workPositionName);

        Task<IEnumerable<EmployeeDTO>> GetEmployeesByRoleAsync(string roleName);

        Task AddNewEmployeeAsync(Employee employee);

        Task<IEnumerable<WorkPosition>> GetWorkPositionsAsync();

        Task<IEnumerable<Role>> GetRolesAsync();

        Task<bool> IsUsernameTakenAsync(string username);
        Task DeleteEmployeeAsync(int employeeId);
        Task UpdateEmployeeAsync(Employee employee);
        Task<EmployeeLoginDTO> LogInWithCredentialsAsync(string username, string password);
        Task<EmployeeLoginDTO> LogInWithQRCodeAsync(string qrCode);
    }
}
