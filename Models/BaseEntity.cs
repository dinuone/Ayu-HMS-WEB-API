using System.ComponentModel.DataAnnotations.Schema;

namespace BookInventory.Models
{
    public class BaseEntity
    {
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("is_delete")]
        public bool IsDelete { get; set; } = false;


        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
    }
}
