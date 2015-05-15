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
            CreateKMLFile(kmlDataType.GenerateKML(url), fileName);
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
