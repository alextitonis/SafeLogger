using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Safe_Logger
{
    class LicenseChecker
    {
        public static LicenseChecker getInstance; public static void init() { getInstance = new LicenseChecker(); }

        string url = @"http://safe-logger.com/t/k.txt";

        static readonly string PasswordHash = "GJDOEJF*#f8F";
        static readonly string SaltKey = "G!JG%LQA";
        static readonly string VIKey = "@7C5c3H8e6F6g7J0";

        public bool exists(string license)
        {
            return true;
            bool final = false;

            if (string.IsNullOrEmpty(license)) 
                return final;

            var result = GetFileViaHttp(url);
            string str = Encoding.UTF8.GetString(result);
            string[] strArr = str.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            string line = "";

            for (int i = 0; i < strArr.Length; i++)
            {
                line = strArr[i];
                if (Decrypt(line) == license)
                {
                    final = true;
                    Utils.license = Decrypt(line);
                }
            }

            return final;
        }

        public byte[] GetFileViaHttp(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadData(url);
            }
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

    }
}
