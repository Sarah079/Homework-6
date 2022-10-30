using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework_6.Models.ViewModels
{
    public class Datasets
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public string borderWidth { get; set; }
        public int[] data
        {
            get; set;
        }
        
    }
}