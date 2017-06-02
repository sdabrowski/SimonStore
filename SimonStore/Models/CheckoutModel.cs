using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimonStore.Models
{
    public class CheckoutModel
    {
        public CheckoutModel()
        {
            this.BillingAddress = new AddressModel();
            this.ShippingAddress = new AddressModel();
        }

        [Required]
        [Display(Name = "Billing Address")]
        public AddressModel BillingAddress { get; set; }
        [Required]
        [Display(Name = "Shipping Address")]
        public AddressModel ShippingAddress { get; set; }
        [Display(Name = "Recipient Name")]
        public string ShippingToLine { get; set; }
        [Phone]
        [Display(Name = "Phone Number")]
        public string ContactPhone { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string ContactEmail { get; set; }

        [Required]
        [Display(Name = "Credit Card Name")]
        public string CreditCardName { get; set; }
        [Required]
        [CreditCard]
        [Display(Name = "Credit Card Number")]
        public string CreditCardNumber { get; set; }
        [Required]
        [MinLength (3)]
        [MaxLength (4)]
        [Display(Name = "Credit Card Verification Number")]
        public string CreditCardVerificationValue { get; set; }
        [Required]
        [Display(Name = "Credit Card Expiration Month")]
        public int? CreditCardExpirationMonth { get; set; }
        [Required]
        [Display(Name = "Creidt Card Expiration Year")]
        public int? CreditCardExpirationYear { get; set; }
        
    }
}