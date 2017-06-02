using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonStore.Models
{
    public class CartProductModel : ProductModel
    {
        public int Quantity { get; set; }
    }
}