using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using website.Models;

namespace website.Areas.Admin.Controllers
{
    public class HomesController : Controller
    {
        // GET: Admin/Homes
        dbcontext db = new dbcontext();
        public ActionResult Index(int? page)
        {
            var lstIdNCC = db.NCCs.Select(x => (int?)x.Id).ToList();
            var lstIdSP = db.SanPhams.ToList();
            List<KeyValuePair<int, int>> a = new List<KeyValuePair<int, int>>();
            foreach (var item in lstIdSP)
            {
                var sp = db.DonHangCTs.Where(G => G.IdSP == item.Id);
                if (sp.Count() > 0)
                {
                    var soluong = sp.Sum(g => g.SoLuong);
                    a.Add(new KeyValuePair<int, int>(Convert.ToInt32(item.IdNCC), soluong));
                }    
               
            }
            var max = 0;
            var Idmax = 0;
            foreach (var item in a)
            {
                if (max < item.Value)
                {
                    max = item.Value;
                    Idmax = item.Key;
                }
            }
            decimal tong = 0;
            var tongsp = 0;
            decimal tonghn = 0;
            var lstIddh = db.DonHangs.Where(g => g.TinhTrang == 2).Select(g =>(int?) g.Id).ToList();
            var chitiet = db.DonHangCTs.Where(g=>lstIddh.Contains(g.IdDH)).ToList();
            foreach (var item in chitiet)
            {
                var gia = db.SanPhams.FirstOrDefault(g => g.Id == item.IdSP);
                var km = db.KhuyenMais.FirstOrDefault(g => g.Id == gia.IdKM).TiLeKM;
                tong += item.SoLuong *Convert.ToDecimal(Convert.ToDouble(gia.GiaGoc)*(100-km)/100);
                tongsp += item.SoLuong;
            }
            var lstIdDH = db.DonHangs.Where(g => g.NgayDat.Day == DateTime.Now.Day&& g.NgayDat.Month == DateTime.Now.Month&& g.NgayDat.Year == DateTime.Now.Year && g.TinhTrang==2).Select(g => (int?)g.Id).ToList();
            foreach (var item in chitiet.Where(g => lstIdDH.Contains(g.IdDH)))
            {
                var gia = db.SanPhams.FirstOrDefault(g => g.Id == item.IdSP);
                var km = db.KhuyenMais.FirstOrDefault(g => g.Id == gia.IdKM).TiLeKM;
                tonghn += item.SoLuong * Convert.ToDecimal(Convert.ToDouble(gia.GiaGoc) * (100 - km) / 100);
            }
            ViewBag.tong = tong;
            ViewBag.tongsp = tongsp;
            ViewBag.tonghn = tonghn;
            ViewBag.ten = db.NCCs.FirstOrDefault(g => g.Id == Idmax).TenNCC;
            var lstId = db.DonHangCTs.Select(g => g.IdSP).ToList();
            ViewData["dm"] = db.DanhMucSPs.ToList();
            ViewData["km"] = db.KhuyenMais.ToList();
            ViewData["dh"] = db.DonHangCTs.ToList();
            ViewData["dhh"] = db.DonHangs.ToList();
            ViewData["ncc"] = db.NCCs.ToList();
            var list = db.SanPhams.Where(g => lstId.Contains(g.Id)).ToList();
            if (page == null) page = 1;
            var books = list.OrderBy(g => g.Id);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = page;
            ViewBag.total = list.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }

    }
}