using System.Text;

namespace BookInventory.Helpers.Utils
{
    public class SupportUtils
    {
        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }


        public static string GenerateOTP()
        {
            Random random = new Random();

            return random.Next(1000, 9999).ToString();
        }
    }
}
