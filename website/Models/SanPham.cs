//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace website.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            this.DanhGias = new HashSet<DanhGia>();
            this.DonHangCTs = new HashSet<DonHangCT>();
        }
    
        public int Id { get; set; }
        public string TenSP { get; set; }
        public string HinhAnh { get; set; }
        public decimal GiaGoc { get; set; }
        public string MoTa { get; set; }
        public string DonViTinh { get; set; }
        public Nullable<int> IdNCC { get; set; }
        public Nullable<int> IdDM { get; set; }
        public Nullable<int> IdKM { get; set; }
        public Nullable<System.DateTime> NgayDang { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }
        public virtual DanhMucSP DanhMucSP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHangCT> DonHangCTs { get; set; }
        public virtual KhuyenMai KhuyenMai { get; set; }
        public virtual NCC NCC { get; set; }
    }
}
