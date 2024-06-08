using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using website.Models;

namespace website.Areas.Admin.Controllers
{
    public class DanhGiaController : Controller
    {
        // GET: Admin/DanhGia
        dbcontext db= new dbcontext();
        public ActionResult Index(int? page)
        {
            var list = db.DanhGias.ToList();
            if (page == null) page = 1;
            var books = list.OrderBy(g => g.NgayDG);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewData["sp"] = db.SanPhams.ToList();
            ViewBag.pageNumber = page;
            ViewBag.total = list.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        public JsonResult Duyet(int? id)
        {
            try
            {
                DanhGia d = db.DanhGias.Find(id);
                d.IsDuyet = true;
                db.DanhGias.AddOrUpdate(d);
                db.SaveChanges();
                return Json(new { status = true, message = "Duyệt bình luận thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = true, message = "Lỗi dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { status = true, message = "Đánh giá không tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            DanhGia banner = db.DanhGias.Find(id);
            if (banner == null)
            {
                return Json(new { status = true, message = "Đánh giá đã bị xóa." }, JsonRequestBehavior.AllowGet);
            }
            db.DanhGias.Remove(banner);
            db.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công." }, JsonRequestBehavior.AllowGet);
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
            return Json(new { status = true, message = "Giao hàng thành công." }, JsonRequestBehavior.AllowGet);
        }
    }
}