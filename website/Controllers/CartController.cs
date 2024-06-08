using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using website.Models;

namespace website.Controllers
{
    public class CartController : Controller
    {
        dbcontext _db = new dbcontext();
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        ////phuong thuc add item vao gio hang
        public JsonResult AddtoCart(int id, int SoLuong)
        {
            var pro = _db.SanPhams.FirstOrDefault(s => s.Id == id);
            //GIOHANG a = new GIOHANG();
            if (pro != null)
            {
                GetCart().Add(pro, SoLuong);
                Cart cart = Session["Cart"] as Cart;
                CartItem c = new CartItem();
                return Json(new { status = true, message = "Thêm giỏ hàng thành công." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = "Thêm giỏ hàng thất bại." }, JsonRequestBehavior.AllowGet);

        }
        ////trang gio hang
        public ActionResult ShowToCart()
        {
            var tk = Convert.ToInt32(Session["Id"]);
            var hoten = _db.ThongTins.FirstOrDefault(g => g.IdTK == tk).HoTen;
            ViewBag.HoTen = hoten;
            if (Session["Cart"] == null)
            {

                return RedirectToAction("Index", "Home");

            }
            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }
        public ActionResult Update_soluong(FormCollection form, int? ID_GioHang = 0)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["ID_SP"]);
            int Sluong = int.Parse(form["So_luong"]);
            var tk = Convert.ToInt32(Request.Cookies["myCookie"].Value);
            var list = _db.DonHangCTs.Where(g => g.IdSP == id_pro ).ToList();
            foreach (var item in list)
            {
                var objSP = _db.SanPhams.FirstOrDefault(g => g.Id == item.IdSP).IdDM;
            }
            cart.UpdateSoluong(id_pro, Sluong);
            return RedirectToAction("ShowToCart", "ShoppingCart");

        }
        public JsonResult Xoa(int Id = 0,int IdBNT=0,int IdMau=0, int IdNCC =0)
        {
                Cart cart = Session["Cart"] as Cart;
            try
            {
                cart.Xoa(Id);
                return Json(new { status = false, message = "Xóa sản phẩm khỏi giỏ hàng thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Xóa thất bại." }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}