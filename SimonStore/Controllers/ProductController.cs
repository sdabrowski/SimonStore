using SimonStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimonStore.Controllers
{
    public class ProductController : Controller
    {

        protected SimonStoreEntities entities = new SimonStoreEntities();
        protected override void Dispose(bool disposing)
        {
            entities.Dispose();
            base.Dispose(disposing);
        }


        // GET: Product
        public ActionResult Index(string id)
        {
            if (!entities.Products.Any(x => x.SKU == id))
            {
                return HttpNotFound("Product doesn't exist");
            }
            else
            {
                return View(entities.Products.Find(id));
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
        public ActionResult Index(Product model, int? quantity)
        {

            Order order = CurrentOrder;
            OrderedProduct orderedProduct = order.OrderedProducts.FirstOrDefault(x => x.SKU == model.SKU);
            if (orderedProduct != null)
            {
                orderedProduct.Quantity += quantity ?? 1;
            }
            else
            {
                orderedProduct = new OrderedProduct
                {
                    SKU = model.SKU,
                    Quantity = quantity ?? 1,
                    ProductPrice = model.Price,
                    Weight = model.Weight
                };
                order.OrderedProducts.Add(orderedProduct);
            }
            entities.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }

        protected Order CurrentOrder
        {
            get
            {
                Order order = null;
                //if (User.Identity.IsAuthenticated)
                //{
                //    b = entities.AspNetUsers.FirstOrDefault(x => x.UserName == User.Identity.Name).Baskets.FirstOrDefault();
                //}
                //else 
                if (Request.Cookies.AllKeys.Contains("cart"))
                {
                    int orderId = int.Parse(Request.Cookies["cart"].Value);
                    order = entities.Orders.Find(orderId);
                }

                if (order == null)
                {
                    order = new Order();
                    //if (User.Identity.IsAuthenticated)
                    //{
                    //    b.AspNetUserID = entities.AspNetUsers.FirstOrDefault(x => x.UserName == User.Identity.Name).Id;
                    //}
                    entities.Orders.Add(order);
                    entities.SaveChanges();
                    //if (!User.Identity.IsAuthenticated)
                    //{
                        Response.Cookies.Add(new HttpCookie("cart", order.OrderID.ToString()));
                    //}
                }
                return order;
            }
        }
    }
}