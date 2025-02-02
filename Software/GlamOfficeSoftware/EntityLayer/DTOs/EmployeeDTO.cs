using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string PIN { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string WorkPositionName { get; set; }
    }
}
