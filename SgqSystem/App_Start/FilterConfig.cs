﻿using Helper;
using SgqSystem.Helpers;
using System.Web.Mvc;

namespace SgqSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new HandleController());
        }
    }
}
