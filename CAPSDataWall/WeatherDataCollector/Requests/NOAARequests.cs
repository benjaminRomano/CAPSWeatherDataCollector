using System;
using System.Net;
using System.Threading.Tasks;
using WeatherDataCollector.Constants;

namespace WeatherDataCollector.Requests
{
    public class NOAARequests
    {
        //Legacy code uncomment if decided to add back severeWeather
        /*
        public static void SevereWeatherRequest(IStorageProvider storageProvider, DateTime currentTime)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(new Uri(WeatherDataConstants.SevereWeatherUrl));
            var response = (HttpWebResponse)request.GetResponse();

            HTTPRequestHelpers.CopyResponseToFile(response, "tempWarningData.kmz");

            var storageUrl = storageProvider.Add("severeWeatherWarningData", currentTime + ".kmz", "tempWarningData.kmz", response.ContentType);

            WeatherAPIRequests.addCountryWeatherOverlay(storageUrl,WeatherOverlayTypes.SevereWeather,currentTime);
        }
        */

        public static bool RadarDataRequest(IStorageProvider storageProvider,string tempFileName)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(new Uri(WeatherDataConstants.RadarDataUrl), tempFileName);

                }
            }
            catch (WebException e)
            {
                return false;
            }

            return true;
        }
    }
}
