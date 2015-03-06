using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPIModels.StreamNames
{
    public class RootStreamName : BaseStreamName
    {
        public override string Name
        {
            get { return "Root"; }
        }
    }
}
