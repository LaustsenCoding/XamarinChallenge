using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinChallenge.Interfaces
{
    public interface IPlatform
    {
        Task<Stream> GetUploadFileAsync();
    }
}
