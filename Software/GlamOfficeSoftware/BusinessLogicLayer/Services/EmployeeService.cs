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
           using(var repo = new EmployeeRepository())
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

            using(var repo = new EmployeeRepository())
            {
                var employee = await repo.GetEmployeeByUsernameAndPasswordAsync(username, password);
                if(employee == null)
                {
                    return null;
                }
                return ConvertEmployeeToDTO(employee);
            }
        }
        private (string username, string pass) DecodeQRCode(string qrCode)
        {
            var data = qrCode.Split(':');
            if(data.Length != 2)
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

    }
}
