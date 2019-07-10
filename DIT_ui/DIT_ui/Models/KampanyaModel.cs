using System;
using System.Collections.Generic;
using System.Text;

namespace DIT_ui.Models
{
    public class KampanyaModel
    {
        
        public string campaignType { get; set; }
        public int campaignId { get; set; }
        public int productId { get; set; }
        public string campaignDetail { get; set; }
        public DateTime validationTime { get; set; }
        public string productName { get; set; }

    }
}
