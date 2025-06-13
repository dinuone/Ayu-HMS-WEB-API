using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harendra_Ayurveda_HMS_WEB_API.Models.Location
{
    [Table("district")]
    public class DistrictModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("province_id")]
        public int ProvinceId { get; set; }
    }
}
