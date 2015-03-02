using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataCollector.StorageProvider
{
    public class StorageProviderAddParams
    {
        public string ServerFolderName { get; set; }
        public string ServerFileName { get; set; }
        public string LocalFileName { get; set; }
        public string ContentType { get; set; }

    }
}
