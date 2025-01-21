using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFactory
{
    public abstract class PdfFactory<T> : IPdfFactory<T> where T : class
    {
        public abstract Task<byte[]> GeneratePdf(T data);
        public abstract Task<string> GenerateStr(T data);
    }
}
