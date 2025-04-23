using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Helpers
{
    public class GlobalAttributeHelper
    {
        public static MySqlConfiguration mySqlConfiguration = new MySqlConfiguration();
        public static AppSettings appSettings = new AppSettings();

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

        public class PatientVisitStatus
        {
            public static string PENDING { get { return "PENDING"; } private set { } }
            public static string ONGOING { get { return "ONGOING"; } private set { } }
            public static string COMPLETE { get { return "COMPLETE"; } private set { } }
        }

    }
}
