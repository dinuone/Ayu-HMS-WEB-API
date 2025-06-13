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
            public static string OPD_SEQUENCE { get { return "OPD_SEQUENCE"; } private set { } }
            public static string CLINIC_SEQUENCE { get { return "CLINIC_SEQUENCE"; } private set { } }
           


        }

        public class PatientVisitStatus
        {
            public static string PENDING { get { return "PENDING"; } private set { } }
            public static string ONGOING { get { return "ONGOING"; } private set { } }
            public static string COMPLETE { get { return "COMPLETE"; } private set { } }
        }

        public class PrescriptionAttribute
        {
            public int id { get; set; }
            public string name { get; set; }
            public int qty { get; set; }
            public string instruction { get; set; }
        }

        public class TreatmentsAttribute
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class ClinicAttribute
        {
            public int id { get; set; }

            public string name { get; set; }

        }

        public class DiagnosisAttribute
        {
            public int id { get; set; }
            public string name { get; set; }
        }



      
        public enum PatientVisitTypes
        {
            Clinic =1,
            Treatment =2
        }

        public enum PatientType
        {
            Feelo = 1,
            Normal =2,
        }

        

        public class DaysAndShift
        {
            public string Day { get; set; }
            public string Shift { get; set; }
        }

        public enum AssignUnit
        {
            CLINIC = 1,
            TREATMENTS = 2,
        }

    }
}
