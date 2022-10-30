using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using Homework_6.Models;
using PagedList;
using Homework_6.Models.ViewModels;

namespace Homework_6.Controllers
{
    public class productsController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        // GET: products
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var products = db.products.Include(p => p.brand).Include(p => p.category);
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.product_name.Contains(searchString));
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(products.ToList().ToPagedList(pageNumber, pageSize));
        }
        //Json results:

        public JsonResult Edit(int? id)
        {
            product product = db.products.Find(id);
            var thisProduct = new product
            {
                product_id = product.product_id,
                product_name = product.product_name,
                category_id = product.category_id,
                brand_id = product.brand_id,
                list_price = product.list_price,
                model_year = product.model_year,
                brands = db.brands.ToList().Select(x => new brand { brand_id = x.brand_id, brand_name = x.brand_name }).ToList(),
                categories = db.categories.ToList().Select(x => new category { category_id = x.category_id, category_name = x.category_name }).ToList()
            };
            return new JsonResult { Data = new { product = thisProduct }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Edit([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            var message = "";
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    message = "200 OK";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return new JsonResult { Data = new { status = message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Details(int? id)
        {
            product dbProd = db.products.Where(x => x.product_id == id).FirstOrDefault();
            DetailsVM newProd = new DetailsVM();
            newProd.product_name = dbProd.product_name;
            newProd.model_year = dbProd.model_year;
            newProd.list_price = dbProd.list_price;
            newProd.brand_name = dbProd.brand.brand_name;
            newProd.category_name = dbProd.category.category_name;
            newProd.quantities = (
                        from stock in db.stocks.ToList()
                        join store in db.stores.ToList() on stock.store_id equals store.store_id
                        where stock.product_id == id
                        group stock by stock.store.store_name into groupedStores
                        select new QuantityVM
                        {
                            store_name = groupedStores.Key,
                            quantity = (int)groupedStores.Sum(oi => oi.quantity)
                        }).ToList();
            return new JsonResult { Data = new { product = newProd }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult Delete(int? id)
        {
            product dbProd = db.products.Where(x => x.product_id == id).FirstOrDefault();
            DetailsVM newProd = new DetailsVM();
            newProd.product_name = dbProd.product_name;
            newProd.model_year = dbProd.model_year;
            newProd.list_price = dbProd.list_price;
            newProd.brand_name = dbProd.brand.brand_name;
            newProd.category_name = dbProd.category.category_name;
            return new JsonResult { Data = new { product = newProd }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteConfirmed(int id)
        {
            var message = "";
            try
            {
                product product = db.products.Find(id);
                db.products.Remove(product);
                db.SaveChanges();
                message = "200 Ok";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return new JsonResult { Data = new { product = message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }




        public ActionResult Create()
        {
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name");
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
