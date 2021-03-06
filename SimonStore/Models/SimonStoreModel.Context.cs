﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SimonStoreEntities : DbContext
    {
        public SimonStoreEntities()
            : base("name=SimonStoreEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoriesProduct> CategoriesProducts { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<OrderedProduct> OrderedProducts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual int sp_AddStoreWithAddress(string street1, string street2, string city, string state, string zip, string storeName)
        {
            var street1Parameter = street1 != null ?
                new ObjectParameter("street1", street1) :
                new ObjectParameter("street1", typeof(string));
    
            var street2Parameter = street2 != null ?
                new ObjectParameter("street2", street2) :
                new ObjectParameter("street2", typeof(string));
    
            var cityParameter = city != null ?
                new ObjectParameter("city", city) :
                new ObjectParameter("city", typeof(string));
    
            var stateParameter = state != null ?
                new ObjectParameter("state", state) :
                new ObjectParameter("state", typeof(string));
    
            var zipParameter = zip != null ?
                new ObjectParameter("zip", zip) :
                new ObjectParameter("zip", typeof(string));
    
            var storeNameParameter = storeName != null ?
                new ObjectParameter("storeName", storeName) :
                new ObjectParameter("storeName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_AddStoreWithAddress", street1Parameter, street2Parameter, cityParameter, stateParameter, zipParameter, storeNameParameter);
        }
    
        public virtual ObjectResult<sp_EmployeeSchedule_Result> sp_EmployeeSchedule(Nullable<System.DateTime> date, Nullable<int> storeID)
        {
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            var storeIDParameter = storeID.HasValue ?
                new ObjectParameter("storeID", storeID) :
                new ObjectParameter("storeID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_EmployeeSchedule_Result>("sp_EmployeeSchedule", dateParameter, storeIDParameter);
        }
    
        public virtual ObjectResult<sp_GetPurchaseByEmail_Result> sp_GetPurchaseByEmail(string email)
        {
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetPurchaseByEmail_Result>("sp_GetPurchaseByEmail", emailParameter);
        }
    
        public virtual ObjectResult<sp_GetStoreRevenue_Result> sp_GetStoreRevenue(Nullable<int> store)
        {
            var storeParameter = store.HasValue ?
                new ObjectParameter("store", store) :
                new ObjectParameter("store", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetStoreRevenue_Result>("sp_GetStoreRevenue", storeParameter);
        }
    
        public virtual ObjectResult<sp_GetStores_Result> sp_GetStores()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetStores_Result>("sp_GetStores");
        }
    }
}
