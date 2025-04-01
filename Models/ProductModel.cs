using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookInventory.Models
{
    public class ProductModel : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Column("bar_code")]
        public string BarCode { get;set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("price", TypeName = "decimal(16,2)")]
        public decimal Price { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }  
        
    }
}
