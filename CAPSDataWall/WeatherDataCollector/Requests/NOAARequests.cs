using System;
using System.Net;
using System.Threading.Tasks;
using WeatherDataCollector.Constants;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector.Requests
{
    public class NOAARequests
    {
        public static bool RadarDataRequest(IStorageProvider storageProvider,string tempFileName)
        {
            return RequestHelper.DownloadFile(WeatherDataConstants.RadarDataUrl, tempFileName);
        }

        public static bool GetTemperatureData(IStorageProvider storageProvider, string tempFileName)
        {
            return RequestHelper.DownloadFile(WeatherDataConstants.TemperatureDataUrl, tempFileName);
        }
    }
}
