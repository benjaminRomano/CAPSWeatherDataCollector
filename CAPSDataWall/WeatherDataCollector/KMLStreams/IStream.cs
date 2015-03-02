using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherDataCollector.KMLFormats;
using WeatherDataCollector.Requests;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.KMLStreams
{
    public interface IStream
    {
        void StartStream();
        void StopStream();

    }
}
