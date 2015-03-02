using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPIModels;
using WeatherDataCollector.Constants;
using WeatherDataCollector.KMLCollector;
using WeatherDataCollector.KMLFormats;
using WeatherDataCollector.KMLStreams;
using WeatherDataCollector.StorageProvider;

namespace WeatherDataCollector
{
    class Collector
    {
        static void Main(string[] args)
        {
            //TODO: Reduce code duplication between latest and historical stream

            //Initialize KML Stream Descriptions
            var latestRadarRoot = new KMLStreamDescription(KMLDataSource.Latest, KMLDataType.Radar, WeatherAPIConstants.RootStream);

            var historicalRadarRoot = new KMLStreamDescription(KMLDataSource.Historical, KMLDataType.Radar, WeatherAPIConstants.RootStream);

            var webRadarRoot = new KMLStreamDescription(KMLDataSource.Web, KMLDataType.Radar, WeatherAPIConstants.RootStream);

            var serverRadarRoot = new KMLStreamDescription(KMLDataSource.Server, KMLDataType.Radar, WeatherAPIConstants.RootStream);
            

            //Initialize Storage Providers
            IPermanentStorageProvider storageProvider = new GoogleDriveStorageProvider(GoogleConstants.GoogleDriveAppClientId,
                GoogleConstants.GoogleDriveAppClientSecret, GoogleConstants.GoogleDriveClientAppName);

            IKMLUseableStorageProvider kmlUseableStorageProvider = new ImgurStorageProvider(ImgurConstants.ClientId);


            //Start Collectors
            var radarCollector = new RadarCollector(storageProvider,serverRadarRoot);
            radarCollector.StartCollector();


            //Start Streams
            var latestRadarStream = new LatestStream(kmlUseableStorageProvider, serverRadarRoot, latestRadarRoot,
                WeatherDataConstants.LatestRadarFileName, TimeSpan.FromMinutes(1));

            latestRadarStream.StartStream();

            var historicalRadarStream = new HistoricalStream(kmlUseableStorageProvider,webRadarRoot,historicalRadarRoot,
                WeatherDataConstants.HistoricalRadarFileName,TimeSpan.FromMinutes(10));

            historicalRadarStream.StartStream();


            Console.ReadLine();

        }
    }
}
