namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisitDTO
{
    public class PatientVisitTimelineDTO
    {
        public long VisitId { get; set; }
        public DateTime VisitDate { get; set; }
        public string DoctorName { get; set; }
        public string VisitType { get; set; }
        public List<string> Treatments { get; set; }
        public List<string> Clinics { get; set; }
    }
}
