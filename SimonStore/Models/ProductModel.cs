using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonStore.Models
{
    //This is how mdoels are typically defined - just a bunch of properties, no (or few) methods.
    //Sometimes referred to as DTOs (Data Transfer Objects), or POCOs (Plain Old CLR Objects).

    public class ProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; } 
    }

    //TODO: This is test data to get everything working - eventually I'm going to put this in a database

    public class ProductData
    {
        public static List<ProductModel> Products;
        static ProductData()
        {
            Products = new List<ProductModel>();
            Products.Add(new ProductModel { ID = 1, Description = "1000' Spool of Black 550 Paracord", Name = "550 Paracord - Black", Price = 38 });
            Products.Add(new ProductModel { ID = 2, Description = "1000' Spool of White 550 Paracord", Name = "550 Paracord - White", Price = 38 });
            Products.Add(new ProductModel { ID = 3, Description = "1000' Spool of Red 550 Paracord", Name = "550 Paracord - Red", Price = 38 });
            Products.Add(new ProductModel { ID = 4, Description = "1000' Spool of Blue 550 Paracord", Name = "550 Paracord - Blue", Price = 38 });
            Products.Add(new ProductModel { ID = 5, Description = "1000' Spool of Green 550 Paracord", Name = "550 Paracord - Green", Price = 38 });
        }
    }

}