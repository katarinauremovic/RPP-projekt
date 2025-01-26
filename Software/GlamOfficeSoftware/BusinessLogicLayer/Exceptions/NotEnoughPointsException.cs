using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exceptions
{
    public class NotEnoughPointsException : Exception
    {
        public NotEnoughPointsException(string message) : base(message)
        {
        }
    }
}
