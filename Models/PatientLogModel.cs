
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Innofactor.EfCoreJsonValueConverter;
﻿using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;


namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    public class PatientLogModel : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("visit_id")]
        public long VisitId { get; set; }

        [Column("visit_date")]
        public DateTime VisitDate { get; set; }

        [Column("patient_id")]
        public long PatientId { get; set; }

        [Column("patient_name")]
        public string PatientName { get; set; }

        [Column("symptoms")]
        public string Symptoms { get; set; }

        [Column("doctor_id")]
        public long DoctorId { get; set; }

        [Column("doctor_name")]
        public string DoctorName { get; set; }

        [Column("chit_number")]
        public string ChitNumber { get; set; }

        [Column("is_assign_to_clinic")]
        public bool IsAssignedToClinic { get; set; }

        [Column("next_visit_date")]
        public DateTime NextVisitDate { get; set; }

        [Column("remark")]
        public string? Remarks { get; set; }

        [JsonField, Column("prescription", TypeName = "json")]
        public List<PrescriptionAttribute>? Prescription { get; set; }

        [JsonField, Column("treatments", TypeName = "json")]
        public List<TreatmentsAttribute>? Treatments { get; set; }

        [JsonField, Column("clinics", TypeName = "json")]
        public List<ClinicAttribute>? Clinics { get; set; }

        [JsonField, Column("diagnosis", TypeName = "json")]
        public List<DiagnosisAttribute>? Diagnosis { get; set; }

        [JsonField, Column("case_sheet", TypeName = "json")]
        public string? CaseSheet { get; set; }

    }
}
