using Newtonsoft.Json.Linq;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using website.Models;

namespace website.Controllers
{
    public class HomeController : Controller
    {
        dbcontext db = new dbcontext();
        public ActionResult Index()
        {
            var idsp = Convert.ToInt32(Request["search"]);
            if (idsp > 0)
            {
                return RedirectToAction("DanhSach", "Home", new { idsp = idsp });
            }
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            ViewData["sanpham"] = db.SanPhams.ToList();
            ViewData["tintuc"] = db.TinTucs.OrderByDescending(g => g.NgayDang).Take(3).ToList();
            ViewData["km"] = db.KhuyenMais.ToList();
            ViewData["ncc"] = db.NCCs.ToList();
            return View();
        }
        public ActionResult Menu()
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            return View();
        }
        public ActionResult GioiThieu()
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            return View();
        }
        public ActionResult LienHe()
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            ViewData["ncc"] = db.NCCs.ToList();
            return View();
        }
        public ActionResult ChiTietSP(int? id)
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            var sp = db.SanPhams.Find(id);
            ViewData["danhgia"] = db.DanhGias.Where(g => g.IsDuyet == true && g.IdSP == id).ToList();
            ViewBag.DM = db.DanhMucSPs.FirstOrDefault(g => g.Id == sp.IdDM).TenDM;
            ViewBag.KM1 = db.KhuyenMais.FirstOrDefault(g => g.Id == sp.IdKM).TiLeKM;
            ViewData["ncc"] = db.NCCs.ToList();
            ViewBag.NCC = db.NCCs.FirstOrDefault(g => g.Id == sp.IdNCC).TenNCC;
            ViewData["sptt"] = db.SanPhams.Where(g => g.IdDM==sp.IdDM).ToList();
            ViewData["km"] = db.KhuyenMais.ToList();
            return View(sp);
        }
        public ActionResult DanhSach(int? id, int? page,string search="",int idsp=0,int sapxep=0,decimal batdau=0,decimal ketthuc=0)
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            ViewData["km"] = db.KhuyenMais.ToList();
            ViewData["ncc"] = db.NCCs.ToList();
            var list = db.SanPhams.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                list=list.Where(g=>g.TenSP.ToLower().Contains(search.ToLower())).ToList();
            }
            if (idsp > 0)
            {
                list = list.Where(g => g.Id == idsp).ToList();
            }
            ViewBag.ten = "";
            var iddm = 0;
            if (id > 0)
            {
                list = list.Where(g => g.IdDM==id).ToList();
                iddm = Convert.ToInt32(id);
                ViewBag.ten = db.DanhMucSPs.FirstOrDefault(g => g.Id == iddm).TenDM;
            }
            ViewBag.id = iddm;
            if (batdau > 0 && ketthuc > 0)
            {
                list = list.Where(g => g.GiaGoc >= batdau && g.GiaGoc <= ketthuc).ToList();
            }
            var books = list.OrderBy(g => g.TenSP);
         
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;
            ViewBag.total = list.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult TinTuc(int? page)
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            var list = db.TinTucs.ToList();
            var books = list.OrderBy(g => g.NgayDang);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;
            ViewBag.total = list.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChiTietTT(int? id)
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            var sp = db.TinTucs.Find(id);
            return View(sp);
        }
      
        [HttpPost]
        public JsonResult DanhGia([Bind(Include = "Id,SDT,Email,BinhLuan,Diem,IdSP")] DanhGia danhgia)
        {
            if (danhgia.Email == "")
            {
                return Json(new { status = false, message = "Email không được bỏ trống." }, JsonRequestBehavior.AllowGet);
            }
            if (danhgia.SDT == "")
            {
                return Json(new { status = false, message = "Số điện thoại không được bỏ trống." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    var mailAddress = new System.Net.Mail.MailAddress(danhgia.Email);
                    danhgia.NgayDG = DateTime.Now;
                    danhgia.IsDuyet = false;
                    db.DanhGias.Add(danhgia);
                    db.SaveChanges();
                    return Json(new { status = true, message = "Cảm ơn quý khách đã để lại để lại đánh giá cho sản phẩm." }, JsonRequestBehavior.AllowGet);
                }
                catch (FormatException)
                {
                    return Json(new { status = false, message = "Email không chính xác." }, JsonRequestBehavior.AllowGet);
                }
            }

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("/Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            return View();
        }
        [HttpPost]
        public JsonResult Login(string Email, string MatKhau)
        {
            if (ModelState.IsValid)
            {
                var user1 = db.TaiKhoans.FirstOrDefault(u => u.TenDangNhap.Equals(Email) && u.MatKhau.Equals(MatKhau));
                if (user1 != null)
                {
                    Session["Id"] = user1.Id;
                    Session["PQ"] = user1.PhanQuyen;
                    Session["Ten"] = db.ThongTins.FirstOrDefault(g => g.IdTK == user1.Id).HoTen;
                    return Json(new { status = true,phanquyen=user1.PhanQuyen, message = "Đăng nhập thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Đăng nhập không thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = true, message = "Thông tin đăng nhập không đủ" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Register()
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            return View();
        }
        [HttpPost]
        public JsonResult Register(string MatKhau, string HoTen, int GioiTinh, string SDT, string Email, string NgaySinh, string DiaChi, string MaTinh)
        {
            try
            {
                if (MatKhau == "" || HoTen == "" || SDT == "" || Email == "" || NgaySinh == "" || NgaySinh == "" || DiaChi == "")
                {
                    return Json(new { status = false, message = "Không được bỏ trống thông tin." }, JsonRequestBehavior.AllowGet);
                }
                else if (Email != "")
                {
                    string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                    Regex regex = new Regex(pattern);
                    var m = regex.IsMatch(Email);
                    if (m == false)
                    {
                        return Json(new { status = false, message = "Email không hợp lệ." }, JsonRequestBehavior.AllowGet);

                    }
                    if (db.TaiKhoans.Any(t => t.TenDangNhap == Email) && db.TaiKhoans.Any(t => t.MatKhau == MatKhau))
                    {
                        return Json(new { status = false, message = "Tài khoản đã tồn tại." }, JsonRequestBehavior.AllowGet);
                    }
                }
                TaiKhoan tk = new TaiKhoan();
                tk.TenDangNhap = Email;
                tk.MatKhau = MatKhau;
                tk.PhanQuyen = 3;
                db.TaiKhoans.Add(tk);
                db.SaveChanges();
                ThongTin kh = new ThongTin();
                kh.HoTen = HoTen;
                kh.Email = Email;
                kh.GioiTinh = GioiTinh;
                kh.SDT = SDT;
                kh.NgaySinh = Convert.ToDateTime(NgaySinh);
                kh.IdTK = tk.Id;
                kh.DiaChi = DiaChi;
                db.ThongTins.Add(kh);
                db.SaveChanges();
                return Json(new { status = true, message = "Đăng kí thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(new { status = false, message = "Lỗi hệ thống." }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ThongTin(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("/Login");
            }
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            ThongTin tt = db.ThongTins.FirstOrDefault(g => g.IdTK == id);
            return View(tt);
        }
        [HttpPost]
        public JsonResult ThongTin(int id, string MatKhau, string HoTen, int GioiTinh, string SDT, string Email, string NgaySinh ,string DiaChi, string MaTinh, int idtk)
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            try
            {
                if (MatKhau == "" || HoTen == "" || SDT == "" || Email == "" || NgaySinh == "" || NgaySinh == "" || DiaChi == "")
                {
                    return Json(new { status = false, message = "Không được bỏ trống thông tin." }, JsonRequestBehavior.AllowGet);
                }
                else if (Email != "")
                {
                    string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                    Regex regex = new Regex(pattern);
                    var m = regex.IsMatch(Email);
                    if (m == false)
                    {
                        return Json(new { status = false, message = "Email không hợp lệ." }, JsonRequestBehavior.AllowGet);
                    }
                }
                ThongTin kh = db.ThongTins.FirstOrDefault(g => g.Id == id);
                kh.HoTen = HoTen;
                kh.Email = Email;
                kh.GioiTinh = GioiTinh;
                kh.SDT = SDT;
                kh.NgaySinh = Convert.ToDateTime(NgaySinh);
                kh.IdTK = idtk;
                kh.DiaChi = DiaChi;
                db.ThongTins.AddOrUpdate(kh);
                db.SaveChanges();
                return Json(new { status = true, message = "Cập nhật thông tin thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(new { status = false, message = "Lỗi hệ thống." }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GioHang()
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            ViewData["km"] = db.KhuyenMais.ToList();
            ViewData["ncc"] = db.NCCs.ToList();
            var Id = Convert.ToInt32(Session["Id"] == null ? 0 : Session["Id"]);
            ThongTin tk = new ThongTin();
            if (Id > 0)
            {
                tk = db.ThongTins.FirstOrDefault(g => g.IdTK == Id);

            }
            Cart cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                return RedirectToAction("Index", "Home", new { sc = 1 });
            }
            return View(tk);
        }
        public ActionResult XoaGH()
        {
            Session["Cart"] = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public JsonResult DatHang(string DiaChi, string SDT, int HinhThuc, double TongTien)
        {
            if (Session["Id"] == null)
            {
                return Json(new { status = false, message = "Cần đăng nhập để đặt hàng" }, JsonRequestBehavior.AllowGet);
            }
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            Cart cart = Session["Cart"] as Cart;
            DonHang dh = new DonHang();
            dh.NgayDat = DateTime.Now;
            dh.DiaChiNhanHang = DiaChi;
            dh.HinhThucTT = HinhThuc;
            dh.IdTK = Convert.ToInt32(Session["Id"]);
            dh.SDT = SDT;
            dh.TongTien =Convert.ToDecimal(TongTien);
            dh.TinhTrang = 1;
            db.DonHangs.Add(dh);
            db.SaveChanges();
            foreach (var item in cart.Items)
            {
                DonHangCT CT = new DonHangCT();
                CT.IdSP = item.product.Id;
                CT.IdDH = dh.Id;
                CT.SoLuong = item.Quantity;
                db.DonHangCTs.Add(CT);
            }
            db.SaveChanges();
            if (HinhThuc == 1)
            {
                Session["Cart"] = null;
                return Json(new { status = true, message = "Đặt hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Session["Cart"] = null;
                return Json(new { status = true, message = "Đặt hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LichSu(string SDT = "")
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            var ct = new List<DonHangCT>();
            ViewData["km"] = db.KhuyenMais.ToList();
            ViewData["sp"] = db.SanPhams.ToList();
            var list= new List<DonHang>();
            if (Session["Id"] != null)
            {
                var Id = Convert.ToInt32(Session["Id"]);
                SDT = db.ThongTins.FirstOrDefault(g => g.IdTK == Id).SDT;
            }
            if (!string.IsNullOrEmpty(SDT))
            {
                list=db.DonHangs.Where(g => g.SDT == SDT).ToList();
                var Iddh = db.DonHangs.Where(g => g.SDT == SDT).Select(g => (int?)g.Id).ToList();
                if (Iddh.Count == 0)
                {
                    return View();
                }
                else
                {
                    ct = db.DonHangCTs.Where(g => Iddh.Contains(g.IdDH)).ToList();
                }
            }
            ViewData["donhang"] = list;
            return View(ct);
        }
        public ActionResult DoiMK()
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            var Id = Convert.ToInt32(Session["Id"]);
            if (Id == 0)
            {
                return RedirectToAction("/Login");
            }
            var tk = db.TaiKhoans.FirstOrDefault(k => k.Id == Id);
            return View(tk);
        }
        [HttpPost]
        public JsonResult DoiMK(string PassCu, string MatKhau, string MatKhauNew)
        {
            ViewData["danhmuc"] = db.DanhMucSPs.ToList();
            if (ModelState.IsValid)
            {
                var Id = Convert.ToInt32(Session["Id"]);
                var tk = db.TaiKhoans.FirstOrDefault(k => k.Id == Id);
                if (PassCu != tk.MatKhau)
                {
                    return Json(new { status = false, message = "Mật khẩu cũ không đúng." }, JsonRequestBehavior.AllowGet);

                }
                else if (MatKhau != MatKhauNew)
                {
                    return Json(new { status = false, message = "Mật khẩu mới không trùng khớp" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tk.MatKhau = MatKhauNew;
                    db.TaiKhoans.AddOrUpdate(tk);
                    db.SaveChanges();
                    return Json(new { status = true, message = "Đổi mật khẩu thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = true, message = "Thông tin không được bỏ trống" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult HuyDon(int? Id)
        {
            var donhang = db.DonHangs.Find(Id);
            donhang.TinhTrang = 3;
            db.DonHangs.AddOrUpdate(donhang);
            db.SaveChanges();
            return Json(new { status = true, message = "Hủy đơn hàng thành công." }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult NhanHang(int? Id)
        {
            var donhang = db.DonHangs.Find(Id);
            donhang.TinhTrang = 2;
            db.DonHangs.AddOrUpdate(donhang);
            db.SaveChanges();
            return Json(new { status = true, message = "Xác nhận đã nhận hàng." }, JsonRequestBehavior.AllowGet);
        }
    }
}