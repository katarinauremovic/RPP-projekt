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

        protected async Task<byte[]> ConvertToPdf(string content)
        {
            return await Task.Run(() => Encoding.UTF8.GetBytes(content));
        }
    }
}
