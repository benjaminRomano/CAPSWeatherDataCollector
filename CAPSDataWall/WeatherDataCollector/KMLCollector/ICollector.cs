using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataCollector.KMLCollector
{
    interface ICollector
    {
        void StartCollector();
        void StopCollector();

    }
}
