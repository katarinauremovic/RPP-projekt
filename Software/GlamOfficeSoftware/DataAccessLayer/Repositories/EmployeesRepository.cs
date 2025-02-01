using DataAccessLayer.Interfaces;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccessLayer.Repositories
{
    public class EmployeesRepository : Repository<Employee>, IEmployeesRepository
    {
        public async override Task<Employee> GetByIdAsync(int id)
        {
            return await items
                
                .FirstOrDefaultAsync(e => e.idEmployee == id);
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await (from emp in context.Employees
                                   join role in context.Roles on emp.Role_idRole equals role.idRole into roleGroup
            from role in roleGroup.DefaultIfEmpty()
                                   join position in context.WorkPositions on emp.WorkPosition_idWorkPosition equals position.idWorkPosition into positionGroup
                                   from position in positionGroup.DefaultIfEmpty()
                                   select new EmployeeDTO
                                   {
                                       Id = emp.idEmployee,
                                       PIN = emp.PIN,
                                       Firstname = emp.Firstname,
                                       Lastname = emp.Lastname,
                                       Email = emp.Email,
                                       Username = emp.Username,
                                       Password = emp.Password,
                                       Salt = emp.Salt,
                                       PhoneNumber = emp.PhoneNumber,
                                       RoleName = role != null ? role.Name : "N/A",
                                       WorkPositionName = position != null ? position.Name : "N/A"
                                   }).ToListAsync();

            return employees;
        }
        

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var employeeDb = await items.FirstOrDefaultAsync(e => e.idEmployee == employee.idEmployee);
            if (employeeDb != null)
            {
                employeeDb.PIN = employee.PIN;
                employeeDb.Firstname = employee.Firstname;
                employeeDb.Lastname = employee.Lastname;
                employeeDb.Email = employee.Email;
                employeeDb.PhoneNumber = employee.PhoneNumber;
                employeeDb.Username = employee.Username;
                employeeDb.Password = employee.Password;
                employeeDb.Role_idRole = employee.Role_idRole;
                employeeDb.WorkPosition_idWorkPosition=employee.WorkPosition_idWorkPosition;
                await SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Employee>> GetEmployeesByName(string name)
        {
            return await items.Where(e => e.Firstname == name).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByLastName(string surname)
        {
            return await items.Where(e => e.Lastname == surname).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByKeyPhrase(string word)
        {
            return await items.Where(e => e.Firstname == word || e.Lastname == word || e.Email == word || e.PhoneNumber == word || e.Username == word).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeeesByWorkingPosition(int workingPosition)
        {
            return await items.Where(e => e.WorkPosition_idWorkPosition == workingPosition).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByRole(int role)
        {
            return await items.Where(e => e.Role_idRole == role).ToListAsync();
        }
    }
}
