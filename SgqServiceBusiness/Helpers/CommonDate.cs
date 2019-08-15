﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqServiceBusiness.Helpers
{
    public class CommonDate
    {
        public static String TransformDateFormatToAnother(String date, String input, String output)
        {
            DateTime dateTime = DateTime.ParseExact(date, input, null);
            return dateTime.ToString(output);
        }

        public static DateTime TransformStringToDateFormat(String date, String format)
        {
            DateTime dateTime = DateTime.ParseExact(date, format, null);
            return dateTime;
        }
    }
}