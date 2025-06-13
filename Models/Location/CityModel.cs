using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harendra_Ayurveda_HMS_WEB_API.Models.Location
{
    [Table("city")]
    public class CityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }


        [Column("district_id")]
        public int DistrictId { get; set; }
    }

}
