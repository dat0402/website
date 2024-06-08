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
    public class TinTucController : Controller
    {
        private dbcontext db = new dbcontext();
        public ActionResult Index(int? page)
        {
            var list = db.TinTucs.ToList();
            if (page == null) page = 1;
            var books = list.OrderBy(g => g.Id);
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
        [ValidateInput(false)]
        public JsonResult Create([Bind(Include = "Id,TieuDe,NoiDung,AnhDaiDien")] TinTuc sanpham)
        {
            try
            {
                var f = Request.Files["AnhDaiDien"];
                if (f != null && f.ContentLength > 0)
                {
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    string UploadPath = Server.MapPath("~/Upload/TinTuc/" + FileName);
                    f.SaveAs(UploadPath);
                    sanpham.AnhDaiDien = FileName;
                }
                sanpham.NgayDang=DateTime.Now;
                db.TinTucs.Add(sanpham);
                db.SaveChanges();
                return Json(new { status = true, message = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
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
                return Json(new { status = true, message = "Tin tức không tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            TinTuc banner = db.TinTucs.Find(id);
            if (banner == null)
            {
                return Json(new { status = true, message = "Tin tức đã bị xóa." }, JsonRequestBehavior.AllowGet);
            }
            string folderPath = Server.MapPath("~/Upload/TinTuc/" + banner.AnhDaiDien);
            if (System.IO.File.Exists(folderPath))
            {
                System.IO.File.Delete(folderPath);
            }
            db.TinTucs.Remove(banner);
            db.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công." }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Edit(int? id)
        {
            TinTuc banner = db.TinTucs.Find(id);
            return PartialView(banner);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit([Bind(Include = "Id,TieuDe,NgayDang,NoiDung,AnhDaiDien")] TinTuc sanpham)
        {
            try
            {
                var f = Request.Files["AnhDaiDien"];
                if (f != null && f.ContentLength > 0)
                {
                    string folderPath = Server.MapPath("~/Upload/TinTuc/" + sanpham.AnhDaiDien);
                    if (System.IO.File.Exists(folderPath))
                    {
                        System.IO.File.Delete(folderPath);
                    }
                    string FileName = System.IO.Path.GetFileName(f.FileName);
                    string UploadPath = Server.MapPath("~/Upload/TinTuc/" + FileName);
                    f.SaveAs(UploadPath);
                    sanpham.AnhDaiDien = FileName;
                }
                db.TinTucs.AddOrUpdate(sanpham);
                db.SaveChanges();
                return Json(new { status = true, message = "Cập nhật dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = true, message = "Lỗi dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}