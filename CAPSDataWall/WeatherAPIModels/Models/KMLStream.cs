using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherAPIModels.Models
{
    public class KMLStream 
    {
        public int ID { get; set; }
        public KMLDataSource Source { get; set; }
        public string Name { get; set; }
        public bool Updated { get; set; }

        public int KMLDataID { get; set; }

        [ForeignKey("KMLDataID")]
        public KMLData KMLData { get; set; }

    }
}
