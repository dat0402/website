using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace website.Models
{
    public class CartItem
    {
        public SanPham product { get; set; }
        public int Quantity { get; set; }
        public double Gia { get; set; }
    }
    public class Cart
    {
        dbcontext db = new dbcontext();
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }
        public void Add(SanPham _sanpham, int _soluong =0)
        {
            var item = items.FirstOrDefault(s => s.product.Id == _sanpham.Id);
            var check = items.FirstOrDefault(g=>g.product.Id==_sanpham.Id);
            if (check == null)
            {
                items.Add(new CartItem
                {
                    product = _sanpham,
                    Quantity = _soluong,
                    Gia =Convert.ToDouble(_sanpham.GiaGoc)
                });
            }
            else
            {
                item.Quantity += _soluong;
            }
            
        }
        public void UpdateSoluong(int id, int _soluong=0)
        {
            var item = items.Find(s => s.product.Id == id);
            if (item != null)
            {
                item.Quantity = _soluong;
            }
        }
        public void Xoa(int Id)
        {
            var item = items.Find(s => s.product.Id == Id);
            items.Remove(item);
        }
        //public double total_money()
        //{
        //    var gi).Gia;
        //    var total = items.Sum(s => (double)gia * (100 - s.product.GiamGia) * 0.01 * s.Quantity);
        //    return (double)total;

        //}
    }
}