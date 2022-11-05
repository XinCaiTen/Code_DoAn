using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class ProductsController : Controller
    {
        private DbContext db = new DbContext();
        //List sản phẩm của thể loại
        public ActionResult Listgenreproduct(int id, int? page = 1)
        {
            int pagesize = 12;//cho phép hiện 12 sản phẩm trên mỗi loại sản phẩm
            int cpage = page ?? 1;
            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && m.genre_id == id).Count();
            List<Product> listgenreproduct = db.Products.Where(n => n.status == "1" && n.genre_id == id).ToList();
            string strName = db.Genres.SingleOrDefault(m => m.status == "1" && m.genre_id == id).genre_name;
            ViewBag.Genrename = strName;
            ViewBag.listgenreproduct = listgenreproduct.ToPagedList(cpage, pagesize);
            return View();
        }
        //List sản phẩm của thương hiệu
        public ActionResult ListbrandProduct(int id, int? page)
        {
            //cho phép hiện 12 sản phẩm trên mỗi loại sản phẩm
            int pagesize = 12;
            int cpage = page ?? 1;
            var genre = db.Genres.Where(m => m.genre_id == id).SingleOrDefault();
            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && m.brand_id == id).Count();
            List<Product> listbrandproduct = db.Products.Where(n => n.brand_id == id && n.status == "1").OrderBy(m => m.product_name).ToList();
            string strBrandName = db.Brands.SingleOrDefault(m => m.brand_id == id && m.status == "1").brand_name;
            ViewBag.Brandname = strBrandName;
            ViewBag.listbrandproduct = listbrandproduct.ToPagedList(cpage, pagesize);
            return View();
        }
        //Danh sách laptop
        public ActionResult Laptop(int? page, string sortOrder)
        {
            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && m.Type == ProductType.LAPTOP).Count();
            ViewBag.Type = "Laptop";
            ViewBag.Urltype = "Laptop";
            return View("Product", GetProduct(m => m.status == "1" && m.Type == ProductType.LAPTOP, page, sortOrder));
        }
        //Code xử lý trang chi tiết sản phẩm
        public ActionResult ProductDetail(int id)
        {
            var product = db.Products.SingleOrDefault(m => m.status == "1" && m.product_id == id);
            List<Product_Image> imageproduct = db.Product_Images.Where(m => m.status == "1" && m.product_id == product.product_id).ToList();
            ViewBag.imageproduct = imageproduct;
            ViewBag.Share = db.Products.Where(m => m.product_id == id);
            ViewBag.CountFeedback = db.Feedbacks.Where(m => m.product_id == id && m.status == "1").Count();
            ViewBag.Feedbackid = db.Feedbacks.OrderByDescending(m => m.feedback_id);
            ViewBag.Accountfeedback = db.Accounts;
            ViewBag.BannerDetail = db.Banner_Detail;
            ViewBag.Banner = db.Banners;
            string Genreproduct = db.Products.SingleOrDefault(m => m.product_id == id && m.status == "1").Genre.genre_name;
            ViewBag.Genreproduct = Genreproduct;
            //Sản phẩm liên quan
            List<Product> relatedproduct = db.Products.Where(item => item.status == "1" && item.product_id != product.product_id && item.genre_id == product.genre_id).Take(4).ToList();
            ViewBag.relatedproduct = relatedproduct;
            List<Feedback> rating = db.Feedbacks.Where(m => m.status == "1").ToList();
            ViewBag.rating = rating;
            return View(product);
        }
        //Danh sách phụ kiện
        public ActionResult Accessory(int? page, string sortOrder)
        {
            ViewBag.Type = "Phụ kiện";
            ViewBag.Urltype = "Accessory";
            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && m.Type == ProductType.ACCESSORY).Count();
            return View("Product", GetProduct(m => m.status == "1" && m.Type == ProductType.ACCESSORY, page, sortOrder));
        }
        //Danh sách linh kiện
        public ActionResult Components(int? page, string sortOrder)
        {
            ViewBag.Type = "Linh kiện";
            ViewBag.Urltype = "Components";
            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && m.Type == ProductType.COMPONENTS).Count();
            return View("Product", GetProduct(m => m.status == "1" && m.Type == ProductType.COMPONENTS, page, sortOrder));
        }
        //Danh sách màn hình
        public ActionResult Monitor(int? page, string sortOrder)
        {
            ViewBag.Type = "Màn hình";
            ViewBag.Urltype = "Monitor";
            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && m.Type == ProductType.MONITOR).Count();
            return View("Product", GetProduct(m => m.status == "1" && m.Type == ProductType.MONITOR, page, sortOrder));
        }
        //Danh sách bàn ghế
        public ActionResult Table(int? page, string sortOrder)
        {
            ViewBag.Type = "Bàn ghế";
            ViewBag.Urltype = "Table";
            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && m.Type == ProductType.DESK).Count();
            return View("Product", GetProduct(m => m.status == "1" && m.Type == ProductType.DESK, page, sortOrder));
        }
        //Tìm kiếm sản phẩm
        public ActionResult SearchResult(string s, int? page, string sortOrder)
        {
            ViewBag.Countproduct = db.Products.Where(m => m.status == "1" && m.product_name.Contains(s)).Count();
            ViewBag.Urltype = "SearchResult";
            ViewBag.Type = "Tìm kiếm";
            return View("Product", GetProduct(m => m.status == "1" && m.product_name.Contains(s), page, sortOrder));
        }
        //gợi ý search
        [HttpPost]
        public JsonResult GetProductSearch(string Prefix)
        {
            //tìm sản phẩm theo tên
            var search = (from c in db.Products
                          where c.status=="1" && c.product_name.StartsWith(Prefix)
                          orderby c.product_name ascending
                          select new { c.product_name });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //Phân trang danh sách sản phẩm
        private IPagedList GetProduct(Expression<Func<Product, bool>> expr, int? page, string sortOrder)
        {
            //ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            //Sắp xếp sản phẩm
            ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
            ViewBag.BuySortParm = sortOrder == "buy_desc" ? "buy_desc" : "buy_desc";
            ViewBag.ViewSortParm = sortOrder == "view_desc" ? "view_desc" : "view_desc";
            ViewBag.UnderthreeMillionSortParm = sortOrder == "duoi-3-trieu" ? "duoi-3-trieu" : "duoi-3-trieu";
            ViewBag.FromthreeToeightMillionSortParm = sortOrder == "tu-3-8-trieu" ? "tu-3-8-trieu" : "tu-3-8-trieu";
            ViewBag.FromeightTofifteenMillionSortParm = sortOrder == "tu-8-15-trieu" ? "tu-8-15-trieu" : "tu-8-15-trieu";
            ViewBag.FromfifteenTotwentyfiveMillionSortParm = sortOrder == "tu-15-25-trieu" ? "tu-15-25-trieu" : "tu-15-25-trieu";
            ViewBag.MorethantwentyfiveMillionSortParm = sortOrder == "tren-25-trieu" ? "tren-25-trieu" : "tren-25-trieu";
            ViewBag.DiscountSortParm = sortOrder == "discount_desc" ? "discount_asc" : "discount_desc";
            ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            //1 trang hiện thỉ tối đa 12 sản phẩm
            int pageSize = 12; 
            int pageNumber = (page ?? 1); 
            var list = db.Products.Where(expr).OrderByDescending(m=>m.product_id).ToPagedList(pageNumber, pageSize);
            switch (sortOrder)
            {
                case "duoi-3-trieu":
                    list = db.Products.Where(expr).OrderByDescending(m => m.product_id).Where(m=>(m.price-m.Discount.discount_price) <3000000).ToPagedList(pageNumber, pageSize);
                    break;
                case "tu-3-8-trieu":
                    list = db.Products.Where(expr).OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) >= 3000000 && (m.price - m.Discount.discount_price) <= 8000000).ToPagedList(pageNumber, pageSize);
                    break;
                case "tu-8-15-trieu":
                    list = db.Products.Where(expr).OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) > 8000000 && (m.price - m.Discount.discount_price) <= 15000000).ToPagedList(pageNumber, pageSize);
                    break;
                case "tu-15-25-trieu":
                    list = db.Products.Where(expr).OrderByDescending(m => m.product_id).Where(m => (m.price - m.Discount.discount_price) > 15000000 && (m.price - m.Discount.discount_price) <= 25000000).ToPagedList(pageNumber, pageSize);
                    break;
                case "tren-25-trieu":
                    list = db.Products.Where(expr).OrderByDescending(m => m.product_id).Where(m =>(m.price - m.Discount.discount_price) >25000000).ToPagedList(pageNumber, pageSize);
                    break;
                case "view_desc":
                    list = db.Products.Where(expr).OrderByDescending(m => m.view).ToPagedList(pageNumber, pageSize);
                    break;
                case "buy_desc":
                    list = db.Products.Where(expr).OrderByDescending(m => m.buyturn).ToPagedList(pageNumber, pageSize);
                    break;
                case "price_asc":
                    list = db.Products.Where(expr).OrderBy(m => (m.price - m.Discount.discount_price)).ToPagedList(pageNumber, pageSize);
                    break;
                case "price_desc":
                    list = db.Products.Where(expr).OrderByDescending(m => (m.price-m.Discount.discount_price)).ToPagedList(pageNumber, pageSize);
                    break;
                case "discount_asc":
                    list = db.Products.Where(expr).OrderBy(m => m.Discount.discount_price).ToPagedList(pageNumber, pageSize);
                    break;
                case "discount_desc":
                    list = db.Products.Where(expr).OrderByDescending(m => m.Discount.discount_price).ToPagedList(pageNumber, pageSize);
                    break;
                case "date_asc":
                    list = db.Products.Where(expr).OrderBy(m => m.Discount.discount_price).ToPagedList(pageNumber, pageSize);
                    break;
                case "name_asc":
                    list = db.Products.Where(expr).OrderBy(m => m.product_name).ToPagedList(pageNumber, pageSize);
                    break;
                case "name_desc":
                    list = db.Products.Where(expr).OrderByDescending(m => m.product_name).ToPagedList(pageNumber, pageSize);
                    break;
                case "date_desc":
                    list = db.Products.Where(expr).OrderByDescending(m => m.product_id).ToPagedList(pageNumber, pageSize);
                    break;
                default: 
                    list = db.Products.Where(expr).OrderByDescending(m => m.product_id).ToPagedList(pageNumber, pageSize);
                    break;
            }
            return list;
        }
    }
}