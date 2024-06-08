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
    public class DonHangController : Controller
    {
        private dbcontext db = new dbcontext();
        public ActionResult Index(int? page,int tinhtrang=-1)
        {
            ViewData["tt"] = db.ThongTins.ToList();
            var list = db.DonHangs.ToList();
            if (tinhtrang > -1)
            {
                list = list.Where(g => g.TinhTrang == tinhtrang).ToList();
            }
            if (page == null) page = 1;
            var books = list.OrderBy(g => g.Id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = page;
            ViewBag.total = list.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        public JsonResult GiaoHang(int? id)
        {
            if (id == null)
            {
                return Json(new { status = true, message = "Đơn hàng không tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            DonHang banner = db.DonHangs.Find(id);
            if (banner == null)
            {
                return Json(new { status = true, message = "Đơn hàng đã bị xóa." }, JsonRequestBehavior.AllowGet);
            }
            banner.TinhTrang = 1;
            db.DonHangs.AddOrUpdate(banner);
            db.SaveChanges();
            return Json(new { status = true, message = "Đơn hàng đã được giao." }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult XemChiTiet(int? page,int id)
        {
            ViewBag.Id = id;
            var lstId = db.DonHangCTs.Where(g => g.IdDH == id).Select(g => g.IdSP).ToList();
            ViewData["dm"] = db.DanhMucSPs.ToList();
            ViewData["km"] = db.KhuyenMais.ToList();
            ViewData["dh"] = db.DonHangCTs.ToList();
            ViewData["ncc"] = db.NCCs.ToList();
            var list = db.SanPhams.Where(g=>lstId.Contains(g.Id)).ToList();
            if (page == null) page = 1;
            var books = list.OrderBy(g => g.Id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = page;
            ViewBag.total = list.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
    }
}