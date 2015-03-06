using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPIModels.StreamNames
{
    public abstract class BaseStreamName 
    {
        public abstract string Name { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
