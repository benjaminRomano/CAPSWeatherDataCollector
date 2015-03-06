using System.IO;
using System.Xml.Serialization;
using WeatherAPIModels.ConvertServices;
using WeatherAPIModels.Models;
using WeatherAPIModels.SpecificModels.KMLDataTypes;

namespace WeatherAPIModels.KMLFormatters
{
    public class KMLFileCreator
    {
        public void CreateKMLFile(KMLDataType type, string url,string fileName)
        {
            var converter = new SpecificConverterService();
            SpecificKMLDataType specificType = converter.ConvertToSpecificKMLData(type);

            CreateKMLFile(specificType.GenerateKML(url), fileName);
        }

        private void CreateKMLFile<T>(T kmlFile, string fileName)
        {
            if (kmlFile == null)
            {
                return;
            }

            var file = File.Create(fileName);
            var serializer = new XmlSerializer(typeof (T));
            serializer.Serialize(file, kmlFile);
            file.Close();
        }

    }
}
