using System;
using System.IO;
using System.Xml.Serialization;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.KMLFormatters
{
    /// <summary>
    /// Creates KML File 
    /// </summary>
    public class KMLFileCreator
    {
        public bool CreateKMLFile(KMLDataType kmlDataType, string url,string fileName)
        {
            var success = true;

            kml kml = null;

            switch (kmlDataType.Name)
            {
                case "Radar":
                {
                    kml = CreateRadarKML(url);
                    break;
                }
            }

            if (kml != null)
            {
                success = CreateKMLFile(kml, fileName);
            }

            return success;
        }

        private kml CreateRadarKML(string url)
        {
            return new kml()
            {
                Document = new kmlDocument()
                {
                    GroundOverlay = new kmlDocumentGroundOverlay()
                    {
                        Icon = new kmlDocumentGroundOverlayIcon()
                        {
                            href = url
                        },
                        LatLonBox = new kmlDocumentGroundOverlayLatLonBox()
                        {
                            north = 50.406626367301044m,
                            south = 21.652538062803m,
                            east = -66.517937876818m,
                            west = -127.620375523875420m
                        },
                        name = "Radar",
                        visibility = 1
                    }
                }
            };
        }

        private bool CreateKMLFile(kml kmlFile, string fileName)
        {
            if (kmlFile == null)
            {
                return false;
            }

            bool success;

            try
            {
                var file = File.Create(fileName);
                var serializer = new XmlSerializer(typeof (kml));
                serializer.Serialize(file, kmlFile);
                file.Close();
                success = true;
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

    }
}
