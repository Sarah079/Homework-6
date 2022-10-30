using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Homework_6.Models;
using Homework_6.Models.ViewModels;
using PagedList;
using System.Text;

namespace Homework_6.Controllers
{
    public class HomeController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        public ActionResult Orders( int? page)
        {
            var order = db.order_items.Include(p => p.product.product_name).Include(o => o.order.order_date).ToString();

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
            return View(order.ToPagedList(pageNumber, pageSize));
        }
        public JsonResult Reporting()
        {
            var data = (from a in db.categories
                        join p in db.products on a.category_id equals p.category_id
                        join i in db.order_items on p.product_id equals i.product_id
                        join o in db.orders on i.order_id equals o.order_id 
                        where a.category_name == "Mountain Bikes" 
                        select new
                        {
                            categoryname = a.category_name,
                            date = o.order_date,
                            quantity = i.quantity
                        }
                         );
            
            Chart _chart = new Chart();
            _chart.labels = data.Select(o => o.date.ToString).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Current Year",
                data = data.Select(x => x.quantity).ToArray(),
                backgroundColor = new string[] { "#FFFFFF", "#000000", "#FF00000" },
                borderColor = new string[] { "#FFFFFF", "#000000", "#FF00000" },
                borderWidth = "1"

            });
            _chart.datasets = _dataSet;
            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
    }
}

