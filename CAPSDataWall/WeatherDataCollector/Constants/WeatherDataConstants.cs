﻿using System;
using System.Collections.Generic;
using System.Security.Policy;
using WeatherAPIModels;

namespace WeatherDataCollector.Constants
{
    public static class WeatherDataConstants
    {
        public const string SevereWeatherUrl = "http://radar.weather.gov/ridge/warningzipmaker.php";
        public const string RadarDataUrl = "http://radar.weather.gov/ridge/Conus/RadarImg/latest_radaronly.gif";
        public const string TemperatureDataUrl = "http://wdssii.nssl.noaa.gov/realtime/metar/recent/Temperature.kmz";

        public const string JsonContent = "application/x-www-form-urlencoded";

        public const string LatestRadarFileName = "latestRadar.kml";
        public const string HistoricalRadarFileName = "historicalRadar.kml";
        public const string TemperatureFileName = "temperature.kmz";

    }
}
