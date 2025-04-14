using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("patients")]
    public class PatientModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("registration_number", TypeName = "varchar(128)")]
        public required string RegistrationNumber { get; set; }

        [Column("qr_code")]
        public string QrCode { get; set; }

        [Column("name", TypeName = "varchar(128)")]
        public required string Name { get; set; }

        [Column("age")]
        public required int Age { get; set; }

        [Column("gender", TypeName = "varchar(64)")]
        public required string Gender { get; set; }

        [Column("weight")]
        public int Weight { get; set; }

        [Column("marital_status", TypeName = "varchar(128)")]
        public string MaritalStatus { get; set; }

        [Column("Occupation", TypeName = "varchar(128)")]
        public string Occupation { get; set; }

        [Column("address_line_1", TypeName = "varchar(128)")]
        public string AddressLine1 { get; set; }

        [Column("address_line_2", TypeName = "varchar(128)")]
        public string AddressLine2 { get; set; }
        [Column("city", TypeName = "varchar(128)")]
        public string City { get; set; }
        [Column("district", TypeName = "varchar(128)")]
        public string District { get; set; }

        [Column("province", TypeName = "varchar(128)")]
        public string Province { get; set; }

        [Column("Patient_type", TypeName = "varchar(64)")]
        public required string PatientType { get; set; }

        [Column("contct_no", TypeName = "varchar(128)")]
        public required string ContactNo { get; set; }


    }
}
