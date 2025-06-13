using System.ComponentModel.DataAnnotations.Schema;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
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

        [Column("updated_by")]
        public string? UpdatedBy { get; set; }  // Tracks who updated the entity

        [Column("updated_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // Automatically set by database
        public DateTime? UpdatedAt { get; set; }
    }
}
