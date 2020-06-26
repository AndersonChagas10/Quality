using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Helper
{
    static public class StringExtensions
    {
        static public string GetFromJsonText(this string text, string propertie)
        {
            var properties = text.Substring(1, text.Length - 2).Split(',');
            for (int i = 0; i < properties.Length; i++)
            {
                if (propertie == properties[i].Split(':')[0].Trim())
                {
                    return properties[i].Split(':')[1];
                }
            }
            return "";
        }
        static public int? GetIntFromJsonText(this string text, string propertie)
        {
            try
            {
                return Convert.ToInt32(text.GetFromJsonText(propertie));
            }
            catch (Exception)
            {
                return null;
            }
        }

        static public string GetSHA256(this string text)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
