using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFactory
{
    public abstract class PdfFactory<T> : IPdfFactory<T> where T : class
    {
        public abstract byte[] GeneratePdf(T data);

        protected byte[] ConvertToPdf(string content)
        {
            return System.Text.Encoding.UTF8.GetBytes(content);
        }
    }
}
