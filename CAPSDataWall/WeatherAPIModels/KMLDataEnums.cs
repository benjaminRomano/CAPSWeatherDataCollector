using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPIModels
{
    public enum KMLDataType
    {
        Radar = 1
    }

    public enum KMLDataSource
    {
        Web = 0,
        Server = 1,
        Latest = 2,
        Historical = 3

    }
}
