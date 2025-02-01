using DataAccessLayer.Interfaces;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data.Entity;
namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : Repository<Employee>
    {
        public async Task<Employee> GetEmployeeByUsernameAndPasswordAsync(string username, string password)
        {
            var employee = await items.FirstOrDefaultAsync(e => e.Username == username);
            if (employee == null)
            {
                return null;
            }

            string hashedPassword = HashPassword(employee.Salt, password);
            if (hashedPassword == employee.Password)
            {
                return employee;
            }
            return null;
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

    }
}
