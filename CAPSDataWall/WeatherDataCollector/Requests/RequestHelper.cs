using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataCollector.Requests
{
    public static class RequestHelper
    {
        public static bool DownloadFile(string url, string fileName)
        {
            try
            {
                using (var webClient = new WebClient())
                {

                    webClient.DownloadFile(new Uri(url), fileName);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Could not download file!");
                return false;
            }
            return true;
        }
    }
}
