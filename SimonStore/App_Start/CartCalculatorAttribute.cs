using SimonStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SimonStore
{
    public class CartCalculatorAttribute : FilterAttribute, IActionFilter
    {
        //This happens after the controller method is called creates cookie for the user to track what is in their cart
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.CartItemCount = 0;

            if (filterContext.RequestContext.HttpContext.Request.Cookies.AllKeys.Contains("cart"))
            {
                using (SimonStoreEntities e = new SimonStoreEntities())
                {
                    HttpCookie cartCookie = filterContext.RequestContext.HttpContext.Request.Cookies["cart"];

                    if (cartCookie == null || cartCookie.Value == "")
                    {
                        filterContext.Controller.ViewBag.CartItemCount = 0;
                    }
                    else
                    {
                        var purchaseId = int.Parse(cartCookie.Value);
                        int quantity = e.Orders.Single(x => x.OrderID == purchaseId).OrderedProducts.Sum(x => (x.Quantity ?? 0));
                        filterContext.Controller.ViewBag.CartItemCount = quantity;
                    }

                }  
            }
        }        
        //This happens before the controller method is called
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}