using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataCollector.StreamGetters
{
    interface IStreamGetter
    {
        void Start();
        void Stop();
    }
}
