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
    public class NCCController : Controller
    {
        dbcontext dbcontext=new dbcontext();
        public ActionResult Index(int? page)
        {
            var list = dbcontext.NCCs.ToList();
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
        public JsonResult Create([Bind(Include = "Id,TenNCC,SDT,DiaChi")] NCC banner)
        {
            try
            {
                dbcontext.NCCs.Add(banner);
                dbcontext.SaveChanges();
                return Json(new { status = true, message = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = true, message = "Lỗi dữ liệu" }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/Banners/Edit/5
        public PartialViewResult Edit(int? id)
        {
            NCC banner = dbcontext.NCCs.Find(id);
            return PartialView(banner);
        }
        [HttpPost]
        public JsonResult Edit([Bind(Include = "Id,TenNCC,SDT,DiaChi")] NCC banner)
        {
            try
            {
                dbcontext.NCCs.AddOrUpdate(banner);
                dbcontext.SaveChanges();
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
            NCC banner = dbcontext.NCCs.Find(id);
            if (banner == null)
            {
                return Json(new { status = true, message = "Banner đã bị xóa." }, JsonRequestBehavior.AllowGet);
            }
            dbcontext.NCCs.Remove(banner);
            dbcontext.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công." }, JsonRequestBehavior.AllowGet);
        }
    }
}