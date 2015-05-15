using System;
using System.Runtime.InteropServices.ComTypes;
using WeatherAPIModels.KMLDataTypes;
using WeatherAPIModels.StreamDescriptions;
using WeatherDataCollector.Constants;
using WeatherDataCollector.Collectors;
using WeatherDataCollector.Requests;
using WeatherDataCollector.StorageProvider;
using WeatherDataCollector.StreamGetters;
using WeatherDataCollector.StreamUpdaters;

namespace WeatherDataCollector
{
    class WeatherDataCollector
    {
        static void Main(string[] args)
        {
            //TODO: Clean up front-end
            //TODO: Create Composite Streams
            //TODO: Add tests to verify changes
            //TODO: Create a latestStreamGetter
            //TODO: Check every 10 seconds if change in getter (Save last kmlDataId)

            const string historicalRootStreamName = "historicalRoot";
            const string latestRootStreamName = "latestRoot";

            //Initialize KMLDataTypes
            var radarKMLDataType = new RadarKMLDataType();
            var temperatureKMLDataType = new TemperatureKMLDataType();
            var irSatelliteKMLDataType = new IRSatelliteKMLDataType();

            //Initialize KML Stream Descriptions
            var latestRadarRoot = new StreamDescription(radarKMLDataType, latestRootStreamName);
            var historicalRadarRoot = new StreamDescription(radarKMLDataType, historicalRootStreamName);

            //Initialize Storage Providers
            IPermanentStorageProvider storageProvider = new GoogleDriveStorageProvider(GoogleConstants.GoogleDriveAppClientId,
                GoogleConstants.GoogleDriveAppClientSecret, GoogleConstants.GoogleDriveClientAppName);

            IKMLUseableStorageProvider kmlUseableStorageProvider = new ImgurStorageProvider(ImgurConstants.ClientId);

            //Start Collectors
            ICollector temperatureCollector = new Collector(storageProvider, temperatureKMLDataType, TimeSpan.FromMinutes(1), time => time.Minute % 10 == 0, NOAARequests.GetTemperatureData);
            temperatureCollector.StartCollector();

            ICollector irSatelliteCollector = new Collector(storageProvider, irSatelliteKMLDataType, TimeSpan.FromMinutes(1), time => time.Minute % 10 == 0, NOAARequests.GetIRSatelliteData);
            irSatelliteCollector.StartCollector();

            //Start latestRadarRoot StreamUpdater and StreamGetter
            IStreamUpdater latestRadarStreamUpdater = new StreamUpdater(latestRadarRoot, TimeSpan.FromMinutes(1));

            latestRadarStreamUpdater.Start();

            IStreamGetter latestRadarStreamGetter = new StreamGetter(kmlUseableStorageProvider, latestRadarRoot,
                WeatherDataConstants.LatestRadarFileName, TimeSpan.FromMinutes(10));

            latestRadarStreamGetter.Start();

            //Start historicalRadarRoot StreamUpdater and StreamGetter
            IStreamUpdater historicalRadarStreamUpdater = new StreamUpdater(historicalRadarRoot, TimeSpan.FromMinutes(1));

            historicalRadarStreamUpdater.Start();

            IStreamGetter historicalRadarStreamGetter = new StreamGetter(kmlUseableStorageProvider, historicalRadarRoot,
                WeatherDataConstants.HistoricalRadarFileName, TimeSpan.FromMinutes(10));

            historicalRadarStreamGetter.Start();

            Console.ReadLine();
        }
    }
}
