using SimonStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimonStore.Controllers
{
    public class CategoryController : Controller
    {

        protected SimonStoreEntities entities = new SimonStoreEntities();
        protected override void Dispose(bool disposing)
        {
            entities.Dispose();
            base.Dispose(disposing);
        }
        // GET: Category
        public ActionResult Index(string id)
        {
            //if(string.IsNullOrEmpty(id))
            //{
            //    return View(Models.ProductData.Products where(XmlSiteMapProvider => XmlSiteMapProvider.Category == id));
            //}
            return View(entities.Products);
        }
    }
}