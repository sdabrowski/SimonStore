using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimonStore.Models;

namespace SimonStore.Controllers
{
    public class ReceiptController : Controller
    {
        protected SimonStoreEntities db = new SimonStoreEntities();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: Receipt
        //May need to see what "purchase" means
        public ActionResult Index(int id)
        {
            var purchase = db.Orders.SingleOrDefault(x => x.OrderID == id);
            return View(purchase);

            //var purchase = db.Orders.SingleOrDefault(x => x.OrderID == id);
            //foreach (var product in purchase.OrderedProducts)
            //{
            //    var modelProduct = purchase.OrderedProducts.FirstOrDefault(x => x.SKU == product.SKU);
            //    product.Quantity = modelProduct.Quantity;

            //}
            //return RedirectToAction("Index");
        }
    }
}