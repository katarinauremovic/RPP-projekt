using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDTO> LogInWithCredentialsAsync(string username, string password);
        Task<EmployeeDTO> LogInWithQRCodeAsync(string qrCode);
    }
}
