using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exceptions
{
    public class ClientOperationException : ApplicationException
    {
        public ClientOperationException(string message) : base(message)
        {
        }

        public ClientOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
