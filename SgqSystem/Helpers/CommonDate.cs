using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Helpers
{
    public class CommonDate
    {
        public static String TransformDateFormatToAnother(String date, String input, String output)
        {
            DateTime dateTime = DateTime.ParseExact(date, input, null);
            return dateTime.ToString(output);
        }
    }
}