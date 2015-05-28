using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPIModels.Models
{
    public class KMLStream 
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int KMLDataId { get; set; }

        [ForeignKey("KMLDataId")]
        public KMLData KMLData { get; set; }
    }
}
