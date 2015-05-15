using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.FileTypes;
using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.KMLDataTypes
{
    public class RadarKMLDataType : KMLDataType
    {
        public RadarKMLDataType(int id = 0, int fileTypeId = 0)
        {
            this.Name = "Radar";
            this.FileType = new GifFileType();
            this.Id = id;
            this.FileTypeId = fileTypeId;
        }

        public override kml GenerateKML(string url)
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
