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
        // GET: Cart
        public ActionResult Index()
        {
            List<Models.CartProductModel> cartProducts = new List<Models.CartProductModel>();
            if (Request.Cookies.AllKeys.Contains("cart"))
            {
                HttpCookie cartCookie = Request.Cookies["cart"];
                //CartCookie comes in with "2,1", meaning productId = 2, quantity = 1
                var cookieValues = cartCookie.Value.Split(',');
                int productID = int.Parse(cookieValues[0].Trim());
                int quantity = int.Parse(cookieValues[1].Trim());
                ProductModel product = ProductData.Products
                    .First(x => x.ID == productID);
                CartProductModel cartProduct = new CartProductModel();
                cartProduct.Description = product.Description;
                cartProduct.ID = product.ID;
                cartProduct.Name = product.Name;
                cartProduct.Price = product.Price;
                cartProduct.Quantity = quantity;

                cartProducts.Add(cartProduct);
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