    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAn_LapTrinhWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //captcha
            routes.IgnoreRoute("{botdetect}",
                new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
            //

            //rút gọn link tìm kiếm sản phẩm
            routes.MapRoute(
              name: "search",
              url: "search",
             defaults: new { Controller = "Products", action = "SearchResult" }
           );
            //------------------------- start rút gọn link chi tiết sản phẩm------------------

            //rút gọn link chi tiết sản phẩm phụ kiện
            routes.MapRoute(
              name: "products detail",
              url: "san-pham/{slug}-{id}",
             defaults: new { Controller = "Products", action = "ProductDetail" }
           );

            //rút gọn link chi tiết sản phẩm phụ kiện
            routes.MapRoute(
              name: "accessory detail",
              url: "phu-kien/{slug}-{id}",
             defaults: new { Controller = "Products", action = "ProductDetail" }
           );
            //rút gọn link chi tiết sản phẩm laptop 
            routes.MapRoute(
              name: "laptop detail",
              url: "laptop/{slug}-{id}",
             defaults: new { Controller = "Products", action = "ProductDetail" }
           );
            //rút gọn link chi tiết sản phẩm linh kiện 
            routes.MapRoute(
              name: "components of computer detail",
              url: "linh-kien/{slug}-{id}",
             defaults: new { Controller = "Products", action = "ProductDetail" }
           );
            //rút gọn link chi tiết sản phẩm bàn ghế
            routes.MapRoute(
              name: "table chairs detail",
              url: "ban-ghe/{slug}-{id}",
             defaults: new { Controller = "Products", action = "ProductDetail" }
           );
            //rút gọn link chi tiết sản phẩm màn hình
            routes.MapRoute(
              name: "monitors detail",
              url: "man-hinh/{slug}-{id}",
             defaults: new { Controller = "Products", action = "ProductDetail" }
           );
//------------------------- end rút gọn link chi tiết sản phẩm--------------------
//------------------------- start rút gọn link danh mục sản phẩm------------------
            //rút gọn link laptop
            routes.MapRoute(
              name: "Laptop",
              url: "laptop",
             defaults: new { Controller = "Products", action = "Laptop" }
           );
            
            //rút gọn link linh kiện
            routes.MapRoute(
              name: "components",
              url: "linh-kien",
             defaults: new { Controller = "Products", action = "components" }
           );

            //rút gọn link màn hình
            routes.MapRoute(
              name: "monitor",
              url: "man-hinh",
             defaults: new { Controller = "Products", action = "Monitor" }
           );

            //rút gọn link bàn/ghế
            routes.MapRoute(
              name: "table-chairs",
              url: "ban-ghe",
             defaults: new { Controller = "Products", action = "Table" }
           );

            //rút gọn link phụ kiện
            routes.MapRoute(
              name: "accessory",
              url: "phu-kien",
             defaults: new { Controller = "Products", action = "Accessory" }
           );

//------------------------- end rút gọn link danh mục sản phẩm------------------

            //rút gọn link list sản phẩm theo thương hiệu
            routes.MapRoute(
              name: "listbrand",
              url: "san-pham/thuong-hieu/{slug}-{id}",
             defaults: new { Controller = "Products", action = "ListbrandProduct" }
           );

            //rút gọn link list sản phẩm theo thương hiệu
            routes.MapRoute(
              name: "listgenre",
              url: "san-pham/phan-loai/{slug}-{id}",
             defaults: new { Controller = "Products", action = "Listgenreproduct" }
           );

            //rút gọn link giỏ hàng
            routes.MapRoute(
                name: "cart",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "ViewCart" }
            );

            //rút gọn link thanh toán giỏ hàng
            routes.MapRoute(
               name: "checkout",
               url: "thanh-toan",
               defaults: new { controller = "Cart", action = "Checkout" }
            );

            //rút gọn link tin tức
            routes.MapRoute(
              name: "news",
              url: "tin-tuc",
              defaults: new { controller = "News", action = "News" }
           );

            //rút gọn link chi tiết tin tức
            routes.MapRoute(
              name: "News detail",
              url: "tin-tuc/abc",
              defaults: new { controller = "News", action = "NewsDetail" }
           );

            //rút gọn link khuyến mãi
            routes.MapRoute(
              name: "promotion",
              url: "khuyen-mai",
              defaults: new { controller = "Discount", action = "Listbanner" }
           );

            //rút gọn link chi tiet sản phẩm khuyến mãi
            routes.MapRoute(
              name: "promotion detail",
              url: "khuyen-mai/{slug}-{id}",
              defaults: new { controller = "Discount", action = "Bannerdetail" }
           );

//------------------------- start rút gọn đăng nhập, đăng ký, thông tin cá nhân,...------------------

            //rút gọn link đăng nhập
            routes.MapRoute(
             name: "signin",
              url: "dang-nhap",
              defaults: new { controller = "Account", action = "SignIn" }
           );
            //rút gọn link đăng ký
            routes.MapRoute(
                name: "registration",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register" }
            );

            //rút gọn link quên mật khẩu
            routes.MapRoute(
              name: "forgotpassword",
              url: "quen-mat-khau",
              defaults: new { controller = "Account", action = "ForgotPassword" }
           );

            //thay đổi mật khẩu
            routes.MapRoute(
              name: "changepassword",
              url: "ca-nhan/doi-mat-khau/",
              defaults: new { controller = "Account", action = "ChangePassword" }
           );

            //rút gọn link thông tin cá nhân
            routes.MapRoute(
              name: "profile",
              url: "ca-nhan/thong-tin",
              defaults: new { controller = "Account", action = "Editprofile" }
           );

            //rút gọn link quản lý đơn hàng
            routes.MapRoute(
              name: "tracking orders",
              url: "ca-nhan/don-hang/",
              defaults: new { controller = "Account", action = "TrackingOrder" }
           );
            //cập nhật mật khẩu mới
            routes.MapRoute(
              name: "Reset password",
              url: "ca-nhan/cap-nhat-mat-khau",
              defaults: new { controller = "Account", action = "ResetPassword" }
           );

//------------------------- end rút gọn đăng nhập, đăng ký, thông tin cá nhân,...------------------
            //gửi yêu cầu hồ trợ
            routes.MapRoute(
              name: "sent request",
              url: "ho-tro",
              defaults: new { controller = "Home", action = "SentRequest" }
           );
            //set error 404
            routes.MapRoute(
              name: "Page Not Found",
              url: "pagenotfound/{id}",
              defaults: new { controller = "Home", action = "PageNotFound", id = UrlParameter.Optional }
           );
            //link mặc định khi khởi động
            routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
