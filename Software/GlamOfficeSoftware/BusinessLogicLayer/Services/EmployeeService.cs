using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        public async Task<EmployeeLoginDTO> LogInWithCredentialsAsync(string username, string password)
        {
            using (var repo = new EmployeeRepository())
            {
                var employee = await repo.GetEmployeeByUsernameAsync(username);
                if (employee == null)
                {
                    return null;
                }
                string hashedPassword = HashPassword(employee.Salt, password);
                var authenticatedEmployee = await repo.GetEmployeeByUsernameAndPasswordAsync(username, hashedPassword);
                if (authenticatedEmployee == null)
                {
                    return null;
                }

                return ConvertEmployeeToDTO(authenticatedEmployee);
            }
        }

        public async Task<EmployeeLoginDTO> LogInWithQRCodeAsync(string qrCode)
        {
            var decoded = DecodeQRCode(qrCode);
            string username = decoded.username;
            string password = decoded.pass;

            using (var repo = new EmployeeRepository())
            {
                var employee = await repo.GetEmployeeByUsernameAndPasswordAsync(username, password);
                if (employee == null)
                {
                    return null;
                }
                return ConvertEmployeeToDTO(employee);
            }
        }
        private (string username, string pass) DecodeQRCode(string qrCode)
        {
            var data = qrCode.Split(':');
            if (data.Length != 2)
            {
                throw new InvalidQRCodeFormatException("Neispravan format QR koda");
            }
            return (data[0], data[1]);
        }
        private string HashPassword(string salt, string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(salt + password);
                byte[] hashedBytes = sha512.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public EmployeeLoginDTO ConvertEmployeeToDTO(Employee employee)
        {
            if (employee == null)
            {
                return null;
            }

            return new EmployeeLoginDTO
            {
                idEmployee = employee.idEmployee,
                Username = employee.Username,
                Role_idRole = employee.Role_idRole
            };
        }
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

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            using (var repo = new EmployeesRepository())
            {
                await repo.UpdateEmployeeAsync(employee);
            }
        }
    }
}
