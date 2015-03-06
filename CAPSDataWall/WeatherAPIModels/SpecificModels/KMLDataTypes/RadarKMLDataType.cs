using WeatherAPIModels.KMLFormatters;
using WeatherAPIModels.Models;
using WeatherAPIModels.SpecificModels.FileTypes;

namespace WeatherAPIModels.SpecificModels.KMLDataTypes
{
    public sealed class RadarKMLDataType : SpecificKMLDataType
    {
        public RadarKMLDataType()
        {
            this.Name = "Radar";
            this.FileType = new GifFileType();
        }

        public RadarKMLDataType(KMLDataType kmlDataType)
        {
            this.Name = "Radar";
            this.FileType = new GifFileType();
            this.ID = kmlDataType.ID;
            this.FileTypeID = kmlDataType.FileTypeID;
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
