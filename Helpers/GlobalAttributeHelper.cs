using Microsoft.EntityFrameworkCore;

namespace BookInventory.Helpers
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

    }
}
