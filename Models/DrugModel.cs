using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("drugs")]
    public class DrugModel : BaseEntity 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name", TypeName = "varchar(128)")]
        public required string Name { get; set; }

        [Column("drug_code", TypeName = "varchar(128)")]
        public required string DrugCode { get; set; }

        [Column("brand", TypeName = "varchar(128)")]
        public string? Brand { get; set; }

        [Column("description", TypeName = "varchar(256)")]
        public string? Description { get; set; }

        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Column("drug_category_id")]
        public long DrugCategoryId { get; set; }

    }
}
