using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exceptions
{
    public class UsernameTakenException : ApplicationException
    {
        public UsernameTakenException(string message) : base(message)
        {
        }
    }
}