using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimonStore.Controllers
{
    public class ProductController : Controller
    {

        // GET: Product
        public ActionResult Index(int? id)
        {
            if (!Models.ProductData.Products.Any(x => x.ID == id))
            {
                return HttpNotFound("Product doesn't exist");
            }
            else
            {
                return View(Models.ProductData.Products.First(x => x.ID == id));
            }
        }



            //if (id == 300)
            //{
            //    //return Redirect("/");
            //}

            //if (string.IsNullOrEmpty(productName))
            //{
            //    productName = "My Product";
            //}

            //var product = new Models.ProductModel { ID = id ?? 0, Name = "My Product", Price = 299m, Description = "This is a product" };

            //Returning JSON is great for passing server-side data back to client-side scripts like Angular or jQuery
            //return Json(product, JsonRequestBehavior.AllowGet);

            //Returning a view will serve up an HTML-based document to the end user which will include my controller data
            //return View(p);

        [HttpPost]
        public ActionResult Index(Models.ProductModel model, int? quantity)
        {
            //TODO: add this product to the current user's cart
            HttpCookie cookie = new HttpCookie("cart", model.ID.ToString() + ", " + quantity.Value.ToString());
            Response.SetCookie(cookie);
            return RedirectToAction("Index", "Cart");
        }
    }
}