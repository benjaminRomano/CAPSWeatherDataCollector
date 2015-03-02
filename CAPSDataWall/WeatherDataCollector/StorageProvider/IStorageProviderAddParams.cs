using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataCollector.StorageProvider
{
    public interface IStorageProviderAddParams
    {
        string ServerFolderName { get; set; }
        string ServerFileName { get; set; }
        string LocalFileName { get; set; }
        string ContentType { get; set; }
    }
}
