﻿using System.Web;
using System.Web.Mvc;

namespace SimonStore
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CartCalculatorAttribute()); //Better idea - cart calculator will now be run on every page!
        }
    }
}
