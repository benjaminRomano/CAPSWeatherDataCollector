using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.Constants
{
    public static class FileTypeDefinitions
    {
        public static FileType GifFileType = new FileType()
        {
            Name = "gif",
            ContentType = "image/gif",
            RequiresKMLFileCreation = true
        };

        public static FileType KMLFileType = new FileType()
        {
            Name = "kml",
            ContentType = "application/vnd.google-earth.kml+xml",
            RequiresKMLFileCreation = false
        };

            public static FileType KMZFileType = new FileType()
        {
            Name = "kmz",
            ContentType = "application/vnd.google-earth.kmz",
            RequiresKMLFileCreation = false
        };
    }
}
