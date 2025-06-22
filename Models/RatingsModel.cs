using System.ComponentModel.DataAnnotations;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    public class RatingsModel:BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }
    }
}
