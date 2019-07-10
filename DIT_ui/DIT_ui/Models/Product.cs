using System;
using System.Collections.Generic;
using System.Text;

namespace DIT_ui.Models
{
    public class Product
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long ProductBarcode { get; set; }
        public int ProductCount { get; set; }
        public double ProductWeight { get; set; }
        public double ProductPrice { get; set; }
        public string ProductUrl { get; set; }
    }
}
