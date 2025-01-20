using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFactory
{
    public interface IPdfFactory<T> where T : class
    {
        byte[] GeneratePdf(T data);
    }
}
