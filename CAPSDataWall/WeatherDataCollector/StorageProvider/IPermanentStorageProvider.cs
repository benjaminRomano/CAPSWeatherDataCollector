using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataCollector.StorageProvider
{
    /// <summary>
    /// KMLStorage where file is never automatically deleted
    /// </summary>
    public interface IPermanentStorageProvider : IStorageProvider
    {
    }
}
