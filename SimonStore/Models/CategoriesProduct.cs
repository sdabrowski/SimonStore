//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimonStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CategoriesProduct
    {
        public int ID { get; set; }
        public int Category { get; set; }
        public string ProductSKU { get; set; }
    
        public virtual Category Category1 { get; set; }
        public virtual Product Product { get; set; }
    }
}
