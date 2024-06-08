using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using website.Models;

namespace website.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        private dbcontext db = new dbcontext();
        public ActionResult Index(int? page)
        {
            var lstId = db.TaiKhoans.Where(g => g.PhanQuyen == 3).Select(g=>(int?)g.Id).ToList();
            var list = db.ThongTins.Where(g=>lstId.Contains(g.IdTK)).ToList();
            if (page == null) page = 1;
            var books = list.OrderBy(g => g.Id);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = page;
            ViewBag.total = list.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public JsonResult Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { status = true, message = "Danh mục menu không tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            TaiKhoan banner = db.TaiKhoans.Find(id);
            if (banner == null)
            {
                return Json(new { status = true, message = "Danh mục menu đã bị xóa." }, JsonRequestBehavior.AllowGet);
            }
            db.TaiKhoans.Remove(banner);
            db.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công." }, JsonRequestBehavior.AllowGet);
        }
    }

}