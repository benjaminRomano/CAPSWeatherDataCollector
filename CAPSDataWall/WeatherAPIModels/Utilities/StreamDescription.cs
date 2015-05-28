namespace WeatherAPIModels.Utilities
{
    /// <summary>
    /// Immutable stream identifier
    /// </summary>
    public class StreamDescription
    {
        public string DataTypeName { get; private set; }
        public string StreamName { get; private set; }

        public StreamDescription(string dataTypeName, string streamName)
        {
            this.DataTypeName = dataTypeName;
            this.StreamName = streamName;
        }

        /// <summary>
        /// Prints out Stream Name and DataType Name
        /// </summary>
        public override string ToString()
        {
            return this.StreamName + " " + this.DataTypeName;
        }
    }
}
