using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using website.Models;

namespace website.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        private dbcontext db = new dbcontext();
        public ActionResult Index(int? page)
        {
            ViewData["dm"] = db.DanhMucSPs.ToList();
            ViewData["km"] = db.KhuyenMais.ToList();
            ViewData["ncc"] = db.NCCs.ToList();
            var list = db.SanPhams.ToList();
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
            ViewData["dm"] = db.DanhMucSPs.ToList();
            ViewData["km"] = db.KhuyenMais.ToList();
            ViewData["ncc"] = db.NCCs.ToList();
            return PartialView();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create([Bind(Include = "Id,TenSP,HinhAnh,GiaGoc,MoTa,DonViTinh,IdNCC,IdDM,IdKM")] SanPham sanpham)
        {

            try
            {
                var f1 = Request.Files.GetMultiple("HinhAnh");
                var hinhanh = "";
                foreach (var f2 in f1)
                {
                    if (f2 != null && f2.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f2.FileName);
                        string UploadPath = Server.MapPath("~/Upload/SanPham/" + FileName);
                        f2.SaveAs(UploadPath);
                        hinhanh = hinhanh + "," + FileName;
                    }
                }
                sanpham.NgayDang = DateTime.Now;
                sanpham.HinhAnh = hinhanh.TrimStart(',').TrimEnd(',');
                db.SanPhams.Add(sanpham);
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
                return Json(new { status = true, message = "Sản phẩm không tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            SanPham banner = db.SanPhams.Find(id);
            if (banner == null)
            {
                return Json(new { status = true, message = "Sản phẩm đã bị xóa." }, JsonRequestBehavior.AllowGet);
            }
            var lst = banner.HinhAnh.Split(',').ToList();
            foreach (var item in lst)
            {
                string folderPath1 = Server.MapPath("~/Upload/SanPham/" + banner.HinhAnh);
                if (System.IO.File.Exists(folderPath1))
                {
                    System.IO.File.Delete(folderPath1);
                }
            }
            db.SanPhams.Remove(banner);
            db.SaveChanges();
            return Json(new { status = true, message = "Xóa thành công." }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Edit(int? id)
        {
            SanPham banner = db.SanPhams.Find(id);
            ViewData["dm"] = db.DanhMucSPs.ToList();
            ViewData["km"] = db.KhuyenMais.ToList();
            ViewData["ncc"] = db.NCCs.ToList();
            return PartialView(banner);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit([Bind(Include = "Id,TenSP,HinhAnh,GiaGoc,MoTa,DonViTinh,IdNCC,IdDM,IdKM,NgayDang")] SanPham sanpham)
        {
            try
            {
                var f1 = Request.Files.GetMultiple("HinhAnh");
                var hinhanh = "";
                if (f1.Count > 0)
                {
                    var lst = sanpham.HinhAnh.Split(',').ToList();
                    foreach (var item in lst)
                    {
                        string folderPath = Server.MapPath("~/Upload/SanPham/" + sanpham.HinhAnh);
                        if (System.IO.File.Exists(folderPath))
                        {
                            System.IO.File.Delete(folderPath);
                        }
                    }

                    foreach (var f2 in f1)
                    {
                        if (f2 != null && f2.ContentLength > 0)
                        {
                            string FileName = System.IO.Path.GetFileName(f2.FileName);
                            string UploadPath = Server.MapPath("~/Upload/SanPham/" + FileName);
                            f2.SaveAs(UploadPath);
                            hinhanh = hinhanh + FileName;
                        }
                    }
                    sanpham.HinhAnh = hinhanh;
                }
                db.SanPhams.AddOrUpdate(sanpham);
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