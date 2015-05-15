using System;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.StreamDescriptions
{
    /// <summary>
    /// Immutable stream identifier
    /// </summary>
    public class StreamDescription
    {
        public KMLDataType KMLDataType { get; private set; }
        public string StreamName { get; private set; }

        public StreamDescription(KMLDataType kmlDataType, string streamName)
        {
            this.KMLDataType = kmlDataType;
            this.StreamName = streamName;
        }

        /// <summary>
        /// Prints out Stream Name and DataType Name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return StreamName + " " + KMLDataType.Name;
        }
    }
}
