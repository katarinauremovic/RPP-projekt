using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exceptions
{
    public class FailedToOpenPdfException : ApplicationException
    {
        public FailedToOpenPdfException(string message) : base(message)
        {
        }
    }
}
