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
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DataAccessLayer.Repositories
{
    public class EmployeesRepository : Repository<Employee>
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
        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByNameAsync(string name)
        {
            var employees = await (from emp in context.Employees
                                   join role in context.Roles on emp.Role_idRole equals role.idRole into roleGroup
                                   from role in roleGroup.DefaultIfEmpty()
                                   join position in context.WorkPositions on emp.WorkPosition_idWorkPosition equals position.idWorkPosition into positionGroup
                                   from position in positionGroup.DefaultIfEmpty()
                                   where emp.Firstname.Contains(name) 
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

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByLastNameAsync(string surname)
        {
            var employees = await (from emp in context.Employees
                                   join role in context.Roles on emp.Role_idRole equals role.idRole into roleGroup
                                   from role in roleGroup.DefaultIfEmpty()
                                   join position in context.WorkPositions on emp.WorkPosition_idWorkPosition equals position.idWorkPosition into positionGroup
                                   from position in positionGroup.DefaultIfEmpty()
                                   where emp.Lastname.Contains(surname)
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

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByKeyPhraseAsync(string word)
        {
            var employees = await (from emp in context.Employees
                                   join role in context.Roles on emp.Role_idRole equals role.idRole into roleGroup
                                   from role in roleGroup.DefaultIfEmpty()
                                   join position in context.WorkPositions on emp.WorkPosition_idWorkPosition equals position.idWorkPosition into positionGroup
                                   from position in positionGroup.DefaultIfEmpty()
                                   where emp.Firstname.Contains(word) ||
                                         emp.Lastname.Contains(word) ||
                                         emp.Email.Contains(word) ||
                                         emp.PhoneNumber.Contains(word) ||
                                         emp.Username.Contains(word)
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

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByWorkPositionAsync(string workPositionName)
        {
            var employees = await (from emp in context.Employees
                                   join role in context.Roles on emp.Role_idRole equals role.idRole into roleGroup
                                   from role in roleGroup.DefaultIfEmpty()
                                   join position in context.WorkPositions on emp.WorkPosition_idWorkPosition equals position.idWorkPosition into positionGroup
                                   from position in positionGroup.DefaultIfEmpty()
                                   where position.Name == workPositionName
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

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeesByRoleAsync(string roleName)
        {
            var employees = await (from emp in context.Employees
                                   join role in context.Roles on emp.Role_idRole equals role.idRole into roleGroup
                                   from role in roleGroup.DefaultIfEmpty()
                                   join position in context.WorkPositions on emp.WorkPosition_idWorkPosition equals position.idWorkPosition into positionGroup
                                   from position in positionGroup.DefaultIfEmpty()
                                   where role.Name == roleName
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

        public async Task<Employee> GetEmployeeByUsernameAsync(string username)
        {
            return await context.Employees.FirstOrDefaultAsync(e => e.Username == username);
        }

    }
}
