using Innofactor.EfCoreJsonValueConverter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("invoice")]
    public class InvoiceModel 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("invoice_no")]
        public string InvoiceNo { get; set; }

        [Column("visit_id")]
        public long VisitId { get;set; }

        [Column("patient_id")]
        public long PatientId { get; set; }

        [JsonField, Column("invoice_description", TypeName = "json")]
        public InvoiceDescription? InvoiceDescription { get; set; }

        [Column("total_amount", TypeName ="decimal(16,2)")]
        public decimal TotalAmount { get; set; }

        [JsonField, Column("sub_total", TypeName = "json")]
        public SubTotal SubTotal { get; set; }

        [Column("doctor_fee", TypeName = "decimal(16,2)")]
        public decimal DoctorFee { get; set; }

        [Column("doctor_id")]
        public long DoctorId { get; set; }

        [Column("payment_method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Column("discount", TypeName = "decimal(16,2)")]
        public decimal Discount { get; set; }

        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
    }

}
