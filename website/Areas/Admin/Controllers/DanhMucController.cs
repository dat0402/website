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
    public class DanhMucController : Controller
    {
        private dbcontext db = new dbcontext();
        public ActionResult Index(int? page)
        {
            var list = db.DanhMucSPs.ToList();
            if (page == null) page = 1;
            var books = list.OrderBy(g => g.Id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = page;
            ViewBag.total = list.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult Create([Bind(Include = "Id,TenDM,MoTa")] DanhMucSP danhmuc)
        {
            try
            {
                db.DanhMucSPs.Add(danhmuc);
                db.SaveChanges();
                return Json(new { status = true, message = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = true, message = "Lỗi dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult Edit(int? id)
        {
            DanhMucSP banner = db.DanhMucSPs.Find(id);
            return PartialView(banner);
        }
        [HttpPost]
        public JsonResult Edit([Bind(Include = "Id,TenDM,MoTa")] DanhMucSP danhmuc)
        {
            try
            {
                db.DanhMucSPs.AddOrUpdate(danhmuc);
                db.SaveChanges();
                return Json(new { status = true, message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
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
                return Json(new { status = true, message = "Danh mục menu không tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            DanhMucSP banner = db.DanhMucSPs.Find(id);
            if (banner == null)
            {
                return Json(new { status = true, message = "Danh mục menu đã bị xóa." }, JsonRequestBehavior.AllowGet);
            }
            db.DanhMucSPs.Remove(banner);
            db.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công." }, JsonRequestBehavior.AllowGet);
        }
    }
}