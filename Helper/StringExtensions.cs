using System;
using System.Globalization;

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
    }
}
