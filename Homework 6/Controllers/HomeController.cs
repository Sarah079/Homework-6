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
using System.Text;

namespace Homework_6.Controllers
{
    public class HomeController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        public ActionResult Orders(string currentFilter, string searchString, int? page)
        {
            var order = db.orders.Include(o => o.order_date).ToString();
            
            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            //ViewBag.CurrentFilter = searchString;
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    order = order.Where(s => s.orders.order_date.Contains(searchString));
            //}

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(order.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Reporting()
        {

            return View();
        }
    }
}

