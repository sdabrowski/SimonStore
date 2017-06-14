﻿using SimonStore.Models;
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
            if (Request.Cookies.AllKeys.Contains("cart"))
            {
                HttpCookie cartCookie = Request.Cookies["cart"];
                var order = entities.Orders.Find(int.Parse(cartCookie.Value));

                return View(order);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Cart
        [HttpPost]
        public ActionResult Index(Order model)
        {
            var order = entities.Orders.Find(model.OrderID);
            foreach(var product in order.OrderedProducts)
            {
                var modelProduct = model.OrderedProducts.FirstOrDefault(x => x.SKU == product.SKU);
                product.Quantity = modelProduct.Quantity;

                if(product.Quantity == 0)
                {
                    //Figure out what to write in this. Ask Joe.
                }
            }
            entities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}