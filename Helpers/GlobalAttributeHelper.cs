using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Helpers
{
    public class GlobalAttributeHelper
    {
        public static MySqlConfiguration mySqlConfiguration = new MySqlConfiguration();
        public static AppSettings appSettings = new AppSettings();
        public static FileUploadSettings fileUploadSettings = new FileUploadSettings();

        public partial class MySqlConfiguration
        {
            public string connectionString;
            public MySqlServerVersion dbVersion;
        }



        public partial class AppSettings
        {
            public string Product;
            public string Version;
            public string Vendor;
            public string Secret;
            public string AppSecret;
            public double TokenDuration;
            public double TokenRefreshDuration;
            public double AgentUploadAttachmentCount;
            public double HoldAttachmentCount;
           
        }

        public partial class FileUploadSettings
        {
            public string BaseUrl { get; set; }
        }

        public static class Sequences
        {
            public static string REGISTRATION_SEQUENCE { get { return "REGISTRATION_SEQUENCE"; } private set { } }
           


        }

        public class PatientVisitStatus
        {
            public static string PENDING { get { return "PENDING"; } private set { } }
            public static string ONGOING { get { return "ONGOING"; } private set { } }
            public static string COMPLETE { get { return "COMPLETE"; } private set { } }
        }

      
        public enum PatientVisitTypes
        {
            OPD =1,
            Clinic =2,
            Treatment =3
        }

        public enum PatientType
        {
            Feelo = 1,
            Normal =2,
        }

        public class PrescriptionAttribute
        {
            public int Id { get; set; }
            public string DrugName { get; set; }
            public int Qty { get; set; }
            public string Instructions { get; set; }
        }

        public class TreatmentsAttribute
        {
            public int Id { get; set; }
            public string TreatmentName { get; set; }
            public string Instructions { get; set; }
        }

        public class ClinicAttribute
        {
            public int Id { get; set; }

            public string ClinicName { get; set; }

        }

        public class DiagnosisAttribute
        {

            public int Id { get; set; }
            public string DiseaseCode { get; set; }
            public string DiseaseName { get; set; }

            public string Description { get; set; }
        }
    }
}
