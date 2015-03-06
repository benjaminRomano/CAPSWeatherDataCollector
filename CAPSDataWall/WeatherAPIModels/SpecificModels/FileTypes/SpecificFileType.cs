using System.ComponentModel.DataAnnotations.Schema;
using WeatherAPIModels.Models;

namespace WeatherAPIModels.SpecificModels.FileTypes
{
    [NotMapped]
    public abstract class SpecificFileType : FileType
    {
    }
}
