using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookInventory.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    public class UserModel : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("first_name", TypeName = "varchar(64)")]
        public string FirstName { get; set; } = string.Empty;

        [Column("last_name", TypeName = "varchar(64)")]
        public string LastName { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("username", TypeName = "varchar(128)")]
        public required string Username { get; set; }

        [Column("password", TypeName ="varchar(128)")]
        public string Password { get; set; }

        [Column("role")]
        public string Role { get; set; }


    }
}
