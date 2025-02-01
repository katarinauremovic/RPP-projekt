using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public static class LoggedInEmployee
    {
        public static EmployeeDTO LoggedEmployee { get; private set; }

        public static bool IsLoggedIn => LoggedEmployee != null;

        public static void SetLoggedInEmployee(EmployeeDTO employee)
        {
            if (employee != null)
            {
                LoggedEmployee = employee;
            }
        }

        public static void Logout()
        {
            LoggedEmployee = null;
        }
    }
}
