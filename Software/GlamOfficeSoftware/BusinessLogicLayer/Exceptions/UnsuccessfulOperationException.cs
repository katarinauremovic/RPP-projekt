﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Exceptions
{
    public class UnsuccessfulOperationException : ApplicationException
    {
        public UnsuccessfulOperationException(string message) : base(message) { }
    }
}
