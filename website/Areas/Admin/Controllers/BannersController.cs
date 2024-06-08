using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using website.Models;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using PagedList;

namespace website.Areas.Admin.Controllers
{
    public class BannersController : Controller
    {
        private dbcontext db = new dbcontext();
        public ActionResult Index(int? page)
        {
            var list = db.Banners.ToList();
            if (page == null) page = 1;
            var books = list.OrderBy(g => g.TieuDe);
            int pageSize = 5;
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
        public JsonResult Create([Bind(Include = "Id,TieuDe,HinhAnh")] Banner banner)
        {
            try
            {
                var f=Request.Files["file"];
                if (f != null && f.ContentLength > 0)
                {
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    string UploadPath = Server.MapPath("~/Upload/Banner/" + FileName);
                    f.SaveAs(UploadPath);
                    banner.HinhAnh= FileName;
                }
                db.Banners.Add(banner);
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
            Banner banner = db.Banners.Find(id);
            return PartialView(banner);
        }
        [HttpPost]
        public JsonResult Edit([Bind(Include = "Id,HinhAnh,TieuDe")] Banner banner)
        {
            try
            {
                var f = Request.Files["file"];
                if (f != null && f.ContentLength > 0)
                {
                    string folderPath = Server.MapPath("~/Upload/Banner/" + banner.HinhAnh);
                    if (System.IO.File.Exists(folderPath))
                    {
                        System.IO.File.Delete(folderPath);
                    }
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    string UploadPath = Server.MapPath("~/Upload/Banner/" + FileName);
                    f.SaveAs(UploadPath);
                    banner.HinhAnh = FileName;
                }
                db.Banners.AddOrUpdate(banner);
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
                return Json(new { status = true, message = "Banner không tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return Json(new { status = true, message = "Banner đã bị xóa." }, JsonRequestBehavior.AllowGet);
            }
            string folderPath = Server.MapPath("~/Upload/Banner/" + banner.HinhAnh);
            if (System.IO.File.Exists(folderPath))
            {
                System.IO.File.Delete(folderPath);
            }
            db.Banners.Remove(banner);
            db.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công." }, JsonRequestBehavior.AllowGet);
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
