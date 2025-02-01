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
        public async Task<Employee> GetEmployeeByUsernameAsync(string username)
        {
            return await items.FirstOrDefaultAsync(e => e.Username == username);
        }
        public async Task<Employee> GetEmployeeByUsernameAndPasswordAsync(string username, string hashedPass)
        {
            var employee = await items.FirstOrDefaultAsync(e => e.Username == username && e.Password == hashedPass);
            return employee;
        }

    }
}
