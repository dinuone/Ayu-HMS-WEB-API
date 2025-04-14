using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookInventory.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("offers")]
    public class OffersModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("name", TypeName = "varchar(128)")]
        public required string Name { get; set; }
        
        [Column("description", TypeName = "varchar(256)")]
        public string? Description { get; set; }
       
        [Column("discount_percentage")]
        public decimal DiscountPercentage { get; set; }
        
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        
        [Column("end_date")]
        public DateTime EndDate { get; set; }
    }
    
}
