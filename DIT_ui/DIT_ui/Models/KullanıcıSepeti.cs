using System;
using System.Collections.Generic;
using System.Text;

namespace DIT_ui.Models
{
   public class KullanıcıSepeti
    {
        public long productId { get; set; }
        public int shoppingCartId { get; set; }
        public int userId { get; set; }
        public DateTime shoppingDate { get; set; }
        public int cartId { get; set; }
        public int productAmount { get; set; }
        public double productPrice { get; set; }
        public double productWeight { get; set; }
        public string productUrl { get; set; }
        public string productName { get; set; }
    }
}
