//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COMICSHOP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChiTietDonHang
    {
        public int IdChiTietDonHang { get; set; }
        public Nullable<int> IdDonHang { get; set; }
        public int MaTruyen { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public double ThanhTien { get; set; }
    
        public virtual DonHang DonHang { get; set; }
        public virtual Truyen Truyen { get; set; }
    }
}