using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Homework_6.Models.ViewModels;

namespace Homework_6.Models
{
    public class DetailsVM
    {
        public string product_name { get; set; }
        public short model_year { get; set; }
        public decimal list_price { get; set; }
        public string brand_name { get; set; }
        public string category_name { get; set; }
        public List<QuantityVM> quantities { get; set; }
    }
}