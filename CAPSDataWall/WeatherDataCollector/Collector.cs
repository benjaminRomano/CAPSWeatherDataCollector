using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPIModels;
using WeatherAPIModels.Constants;
using WeatherAPIModels.Models;
using WeatherAPIModels.SpecificModels.KMLDataTypes;
using WeatherAPIModels.StreamDescriptions;
using WeatherAPIModels.StreamNames;
using WeatherDataCollector.Constants;
using WeatherDataCollector.KMLCollector;
using WeatherDataCollector.Requests;
using WeatherDataCollector.StorageProvider;
using WeatherDataCollector.Streams;

namespace WeatherDataCollector
{
    class Collector
    {
        static void Main(string[] args)
        {
            //TODO: De-abstract Context from Controller
            //TODO: Improve IStream Code

            //Initialize KMLDataTypes
            var radarKMLDataType = new RadarKMLDataType();
            var temperatureKMLDataType = new TemperatureKMLDataType();

            //Initialize Stream Names
            var rootStreamName = new RootStreamName();

            //Initialize KML Stream Descriptions
            var latestRadarRoot = new KMLStreamDescription(KMLDataSource.Latest, radarKMLDataType, rootStreamName);
            var historicalRadarRoot = new KMLStreamDescription(KMLDataSource.Historical, radarKMLDataType, rootStreamName);
            var webRadarRoot = new KMLStreamDescription(KMLDataSource.Web, radarKMLDataType, rootStreamName);
            var serverRadarRoot = new KMLStreamDescription(KMLDataSource.Server, radarKMLDataType, rootStreamName);

            var serverTemperatureRoot = new KMLStreamDescription(KMLDataSource.Server, temperatureKMLDataType, rootStreamName);
            var latestTemperatureRoot = new KMLStreamDescription(KMLDataSource.Latest, temperatureKMLDataType, rootStreamName);
            

            //Initialize Storage Providers
            IPermanentStorageProvider storageProvider = new GoogleDriveStorageProvider(GoogleConstants.GoogleDriveAppClientId,
                GoogleConstants.GoogleDriveAppClientSecret, GoogleConstants.GoogleDriveClientAppName);

            IKMLUseableStorageProvider kmlUseableStorageProvider = new ImgurStorageProvider(ImgurConstants.ClientId);

            //Start Collectors
            ICollector radarCollector = new BaseCollector(storageProvider, serverRadarRoot,TimeSpan.FromMinutes(1), time => time.Minute % 10 == 0, NOAARequests.RadarDataRequest );
            radarCollector.StartCollector();

            ICollector temperatureCollector = new BaseCollector(storageProvider, serverTemperatureRoot, TimeSpan.FromMinutes(1), time => time.Minute% 10 == 0, NOAARequests.GetTemperatureData);
            temperatureCollector.StartCollector();

            //Start Streams
            IStream latestRadarStream = new LatestStream(kmlUseableStorageProvider, serverRadarRoot, latestRadarRoot,
                WeatherDataConstants.LatestRadarFileName, TimeSpan.FromMinutes(1));

            latestRadarStream.StartStream();

            IStream historicalRadarStream = new HistoricalStream(kmlUseableStorageProvider,webRadarRoot,historicalRadarRoot,
                WeatherDataConstants.HistoricalRadarFileName,TimeSpan.FromMinutes(10));

            historicalRadarStream.StartStream();


            IStream latestTemperatureStream = new LatestStream(kmlUseableStorageProvider, serverTemperatureRoot, latestTemperatureRoot, 
                WeatherDataConstants.TemperatureFileName, TimeSpan.FromMinutes(1));

            latestTemperatureStream.StartStream();


            Console.ReadLine();
        }
    }
}
