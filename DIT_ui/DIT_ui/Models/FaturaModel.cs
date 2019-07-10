using System;
using System.Collections.Generic;
using System.Text;

namespace DIT_ui.Models
{   
    public class FaturaModel
    {
        public int faturaID { get; set; }
        public string faturaTutar { get; set; }
        public int userId { get; set; }
        public string body { get; set; }
        public DateTime faturaTarih { get; set; }
    }
}
