using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class EmployeeDTO
    {
        public int idEmployee { get; set; }
        public string Username { get; set; }
        public int? Role_idRole { get; set; }
    }
}
