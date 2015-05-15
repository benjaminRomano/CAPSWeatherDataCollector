namespace WeatherDataCollector.Collectors
{
    /// <summary>
    /// Pulls latest data for dataType
    /// </summary>
    interface ICollector
    {
        void StartCollector();
        void StopCollector();
    }
}
