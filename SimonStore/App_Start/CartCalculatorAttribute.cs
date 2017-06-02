using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimonStore.App_Start
{
    public class CartCalculatorAttribute : FilterAttribute, IActionFilter
    {
        //This happens after the controller method is called
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            int i = 1;
        }

        //This happens before the controlelr method is called
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int i = 1;
        }
    }
}