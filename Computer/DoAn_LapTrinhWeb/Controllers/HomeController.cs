using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
// using DoAn_LapTrinhWeb.Models;


namespace DoAn_LapTrinhWeb.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private DbContext db = new DbContext();

        public ActionResult Index( int ? page)
        {
            int pagesize = 4;//cho phép hiện 4 sản phẩm trên mỗi loại sản phẩm
            int cpage = page ?? 1;
            List<Product> newproduct = db.Products.Where(item => item.status == "1" && item.quantity != "0" && item.Type != ProductType.ACCESSORY).Take(20).OrderByDescending(m => m.product_id).ToList();
            ViewBag.NewProduct = newproduct.ToPagedList(cpage, pagesize);
            //hiển thị những sản phẩm mới được mua nhiều nhất
            List<Product> hotproduct = db.Products.Where(item => item.status == "1"&& item.quantity != "0" && item.Type != ProductType.ACCESSORY).Take(20).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.HotProduct = hotproduct.ToPagedList(cpage, pagesize);
            //hiển thị những linh kiện điện tử được mua nhiều nhất
            List<Product> componentsales = db.Products.Where(item => item.status == "1" && item.quantity != "0" && item.Type == ProductType.COMPONENTS).Take(20).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.ComponentSales = componentsales.ToPagedList(cpage, pagesize);
            //hiển thị những linh kiện điện tử được mua nhiều nhất
            List<Product> monitorsales = db.Products.Where(item => item.status == "1" && item.quantity != "0" && item.Type == ProductType.MONITOR).Take(20).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.MonitorSales = monitorsales.ToPagedList(cpage, pagesize);
            //hiển thị những laptopgaming được mua nhiều nhất
            List<Product> laptopgamingsales = db.Products.Where(item => item.status == "1"&& item.quantity != "0" && item.genre_id==Genre.laptopgaming && item.Type == ProductType.LAPTOP).Take(240).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.LaptopGamingSales = laptopgamingsales.ToPagedList(cpage, pagesize);
            //hiển thị những phụ kiện được mua nhiều nhất
            List<Product> laptopforofficeworksales = db.Products.Where(item => item.status == "1"&& item.quantity != "0" && item.genre_id == Genre.hoctapvanphong && item.Type == ProductType.LAPTOP).Take(20).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.LaptopForOfficeWorkSales = laptopforofficeworksales.ToPagedList(cpage, pagesize);
            //hiển thị những phụ kiện được mua nhiều nhất
            List<Product> laptopforraphicdesignsales = db.Products.Where(item => item.status == "1"&& item.quantity != "0" && item.genre_id == Genre.dohoakithuat && item.Type == ProductType.LAPTOP).Take(20).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.LaptopForGraphicDesignSales = laptopforraphicdesignsales.ToPagedList(cpage, pagesize);
            //hiển thị những linh kiện điện tử được mua nhiều nhất
            List<Product> desksales = db.Products.Where(item => item.status == "1" && item.quantity != "0" && item.Type == ProductType.DESK).Take(24).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.DeskSales = desksales.ToPagedList(cpage, pagesize);
            //hiển thị những phụ kiện được mua nhiều nhất
            List<Product> accessorysales = db.Products.Where(item => item.status == "1" && item.quantity != "0" && item.Type==ProductType.ACCESSORY).Take(24).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.AccessorySales = accessorysales.ToPagedList(cpage, pagesize); ;
            //hiển thị những sản phẩm được mua nhiều nhất theo loại cao cấp sang trọng
            List<Product> premiumlaptopsales = db.Products.Where(item => item.status == "1" && item.genre_id == Genre.caocapsangtrong &&item.Type==ProductType.LAPTOP).Take(24).OrderByDescending(item => item.buyturn).Take(4).ToList();
            ViewBag.PremiumLaptopSales = premiumlaptopsales.ToPagedList(cpage, pagesize);
            //banner khuyến mãi
            ViewBag.BannerHeader = db.Banners.OrderByDescending(m => m.banner_id).Where(m=>m.status=="1" && m.banner_type==1).Take(8).ToList();
            ViewBag.BannerBottom = db.Banners.OrderByDescending(m => m.banner_id).Where(m => m.status == "1" && m.banner_type == 2).Take(4).ToList();
            ViewBag.BannerVertical = db.Banners.OrderByDescending(m => m.banner_id).Where(m => m.status == "1" && m.banner_type == 3).Take(1).ToList();
            return View();       
        }
        //Error 404 hiện khi sai URL
        public ActionResult PageNotFound()
        {
            return View();
        }
        //View Gửi yêu cầu hỗ trợ
        public ActionResult SentRequest()
        {
            return View();
        }
        //Code cử lý Gửi yêu càu hỗ trợ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SentRequest(Contact contact)
        {
            try
            {
                contact.phone = contact.phone;
                contact.content = contact.content;
                contact.name = contact.name;
                contact.image = contact.image;
                contact.email = contact.email;
                contact.flag = 0;
                contact.status = "1";
                contact.create_by = contact.email;
                contact.update_by = contact.email;
                contact.update_at = DateTime.Now;
                contact.create_at = DateTime.Now;
                db.Contacts.Add(contact);
                db.SaveChanges();
                Notification.set_flash("Gửi yêu cầu thành công", "success");
                return View("SentRequest");
            }
            catch
            {
                Notification.set_flash("Gửi yêu cầu thất bại", "danger");
                return View();
            }
        }

    }
}