using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector
{
    public interface IStorageProvider
    {
        string Add(StorageProviderAddParams addParams);
        string Get(string serverFolderName, string serverFileName);
    }
}
