using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WeatherAPIModels;

namespace WeatherDataCollector.KMLFormats
{
    public static class KMLFileCreator
    {

        public static void CreateKMLFile(KMLDataType type, string url,string fileName)
        {
            if (type == KMLDataType.Radar)
            {
                CreateKMLFile(GenerateRadarKML(url), fileName);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void CreateKMLFile<T>(T kmlFile, string fileName)
        {
            var file = File.Create(fileName);
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(file, kmlFile);
            file.Close();
        }

        private static kml GenerateRadarKML(string url)
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
    }
}
