using SimonStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimonStore.Controllers
{
    public class CartController : Controller
    {



        protected SimonStoreEntities entities = new SimonStoreEntities();
        protected override void Dispose(bool disposing)
        {
            entities.Dispose();
            base.Dispose(disposing);
        }
        // GET: Cart
        public ActionResult Index()
        {
            List<Models.CartProductModel> cartProducts = new List<Models.CartProductModel>();
            if (Request.Cookies.AllKeys.Contains("cart"))
            {
                HttpCookie cartCookie = Request.Cookies["cart"];
                var order = entities.Orders.Find(int.Parse(cartCookie.Value));

                cartProducts = order.OrderedProducts.Select(x => new CartProductModel
                {
                    Description = x.Product.Description,
                    ID = 0,
                    Name = x.Product.Name,
                    Price = x.Product.Price,
                    Quantity = x.Quantity ?? 0
                }).ToList();
                
            }
            return View(cartProducts);
        }

        // Post: Cart
        [HttpPost]
        public ActionResult Index(CartProductModel[] model, int? quantity)
        {
            HttpCookie cartCookie = Request.Cookies["cart"];
            //CartCookie comes in with "2,1", meaning productId = 2, quantity = 1
            var cookieValues = cartCookie.Value.Split(',');
            int productId = int.Parse(cookieValues[0].Trim());
            cartCookie.Value = productId + "," + quantity.Value;

            if(quantity == null || quantity.Value < 1)
            {
                cartCookie.Expires = DateTime.UtcNow;
            }
            Response.SetCookie(cartCookie);
            return RedirectToAction("Index");
        }
    }
}