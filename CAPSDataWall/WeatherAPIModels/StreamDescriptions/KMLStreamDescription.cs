using WeatherAPIModels.Models;
using WeatherAPIModels.StreamNames;

namespace WeatherAPIModels.StreamDescriptions
{
    public class KMLStreamDescription
    {
        public KMLDataSource Source { get; set; }
        public KMLDataType KMLDataType { get; set; }
        public BaseStreamName StreamName { get; set; }

        public KMLStreamDescription(KMLDataSource source, KMLDataType kmlDataType, BaseStreamName streamName)
        {
            this.Source = source;
            this.KMLDataType = kmlDataType;
            this.StreamName = streamName;
        }
    }
}
