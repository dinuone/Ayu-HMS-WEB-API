using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookInventory.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    public class BranchModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(128)")]
        public string Name { get; set; }

        [Column("address", TypeName = "varchar(256)")]
        public string Address { get; set; }

        [Column("phone_number", TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; }

      
    }
}
