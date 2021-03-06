﻿using System;
using System.Runtime.InteropServices.ComTypes;
using WeatherAPIModels.KMLDataTypes;
using WeatherAPIModels.Utilities;
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
            //TODO: Re-do website interface since it's broken
            //TODO: Create Composite Streams
            //TODO: Add tests to verify changes
            //TODO: Create a latestStreamGetter
            //TODO: Check every 10 seconds if change in getter (Save last kmlDataId to check if it is different)

            const string historicalRootStreamName = "historicalRoot";
            const string latestRootStreamName = "latestRoot";

            //Initialize KMLDataTypes
            var radarKMLDataType = new RadarKMLDataType();
            var temperatureKMLDataType = new TemperatureKMLDataType();
            var irSatelliteKMLDataType = new IRSatelliteKMLDataType();

            //Initialize KML Stream Descriptions
            var latestRadarRoot = new StreamDescription(radarKMLDataType.Name, latestRootStreamName);
            var historicalRadarRoot = new StreamDescription(radarKMLDataType.Name, historicalRootStreamName);

            //Initialize Storage Providers
            IPermanentStorageProvider storageProvider = new GoogleDriveStorageProvider(GoogleConstants.GoogleDriveAppClientId,
                GoogleConstants.GoogleDriveAppClientSecret, GoogleConstants.GoogleDriveClientAppName);

            IKMLUseableStorageProvider kmlUseableStorageProvider = new ImgurStorageProvider(ImgurConstants.ClientId);

            //Start Collectors
            ICollector temperatureCollector = new Collector(storageProvider, radarKMLDataType, TimeSpan.FromMinutes(1), time => time.Minute % 1 == 0, NOAARequests.GetRadarData);
            temperatureCollector.StartCollector();

            ICollector irSatelliteCollector = new Collector(storageProvider, irSatelliteKMLDataType, TimeSpan.FromMinutes(1), time => time.Minute % 1 == 0, NOAARequests.GetIRSatelliteData);
            irSatelliteCollector.StartCollector();

            //Start latestRadarRoot StreamUpdater and StreamGetter
            IStreamUpdater latestRadarStreamUpdater = new StreamUpdater(latestRadarRoot, TimeSpan.FromMinutes(1));

            latestRadarStreamUpdater.Start();

            IStreamGetter latestRadarStreamGetter = new StreamGetter(kmlUseableStorageProvider, latestRadarRoot,
                WeatherDataConstants.LatestRadarFileName);

            latestRadarStreamGetter.Start();

            //Start historicalRadarRoot StreamUpdater and StreamGetter
            IStreamUpdater historicalRadarStreamUpdater = new StreamUpdater(historicalRadarRoot, TimeSpan.FromMinutes(1));

            historicalRadarStreamUpdater.Start();

            IStreamGetter historicalRadarStreamGetter = new StreamGetter(kmlUseableStorageProvider, historicalRadarRoot,
                WeatherDataConstants.HistoricalRadarFileName);

            historicalRadarStreamGetter.Start();

            Console.ReadLine();
        }
    }
}
