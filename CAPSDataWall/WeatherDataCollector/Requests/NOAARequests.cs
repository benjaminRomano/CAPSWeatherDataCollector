using WeatherDataCollector.Constants;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.Requests
{
    public class NOAARequests
    {
        public static bool GetRadarData(IStorageProvider storageProvider,string tempFileName)
        {
            return RequestHelper.DownloadFile(WeatherDataConstants.RadarDataUrl, tempFileName);
        }

        public static bool GetTemperatureData(IStorageProvider storageProvider, string tempFileName)
        {
            return RequestHelper.DownloadFile(WeatherDataConstants.TemperatureDataUrl, tempFileName);
        }

        public static bool GetIRSatelliteData(IStorageProvider storageProvider, string tempFileName)
        {
            return RequestHelper.DownloadFile(WeatherDataConstants.IRSatelliteDataUrl, tempFileName);
        }
    }
}
