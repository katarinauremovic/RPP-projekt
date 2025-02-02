using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exceptions
{
    public class InvalidEmployeeDataException : ApplicationException
    {
        public InvalidEmployeeDataException(string message) : base(message)
        {
        }
    }
}
