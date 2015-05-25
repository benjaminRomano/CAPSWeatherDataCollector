namespace WeatherAPIModels.Utilities
{
    /// <summary>
    /// Immutable stream identifier
    /// </summary>
    public class StreamDescription
    {
        public string KMLDataTypeName { get; private set; }
        public string StreamName { get; private set; }

        public StreamDescription(string kmlDataTypeName, string streamName)
        {
            this.KMLDataTypeName = kmlDataTypeName;
            this.StreamName = streamName;
        }

        /// <summary>
        /// Prints out Stream Name and DataType Name
        /// </summary>
        public override string ToString()
        {
            return this.StreamName + " " + this.KMLDataTypeName;
        }
    }
}
