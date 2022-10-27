using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Homework_6.Models;
using PagedList;

namespace Homework_6.Controllers
{
    public class HomeController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        public ActionResult Orders()
        {
            string inputdate = "";
            var orders = db.orders.Include(o => o.order_date);
            
            return View();
        }
    }
}
//            var products = db.products.Include(p => p.brand).Include(p => p.category);
//                products = products.Where(s => s.product_name.Contains(searchString));
