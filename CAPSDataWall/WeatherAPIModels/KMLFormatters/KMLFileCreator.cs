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
        public void CreateKMLFile(KMLDataType kmlDataType, string url,string fileName)
        {
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
                CreateKMLFile(kml, fileName);
            }
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

        private void CreateKMLFile(kml kmlFile, string fileName) 
        {
            if (kmlFile == null)
            {
                return;
            }

            var file = File.Create(fileName);
            var serializer = new XmlSerializer(typeof(kml));
            serializer.Serialize(file, kmlFile);
            file.Close();
        }

    }
}
