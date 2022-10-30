using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework_6.Models.ViewModels
{
    public class Chart
    {
        public string[] labels { get; set; }
        public List<Datasets> datasets { get; set; }
    }
}