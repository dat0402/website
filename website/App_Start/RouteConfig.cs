using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("vechungtoi", "ve-chung-toi", new { controller = "Home", action = "GioiThieu" });
            routes.MapRoute("tintuc", "tin-tuc", new { controller = "Home", action = "TinTuc" });
            routes.MapRoute("lienhe", "lien-he", new { controller = "Home", action = "LienHe" });
            routes.MapRoute("danhsach", "danh-sach", new { controller = "Home", action = "DanhSach", id = UrlParameter.Optional});
            routes.MapRoute("danhsachloai", "danh-sach-loai/{id}", new { controller = "Home", action = "DanhSach", id = UrlParameter.Optional});
            routes.MapRoute("chitietsp", "chi-tiet-san-pham/{id}", new { controller = "Home", action = "ChiTietSP", id = UrlParameter.Optional});
            routes.MapRoute("chitiettin", "chi-tiet-tin-tuc/{id}", new { controller = "Home", action = "ChiTietTT", id = UrlParameter.Optional});
            routes.MapRoute("chitietsploai", "chi-tiet-san-pham-loai/{id}", new { controller = "Home", action = "ChiTietSP_Loai", id = UrlParameter.Optional});
            routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
             );
        }
    }
}
