using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMICSHOP.Models.ViewModel
{
    public class ChiTietDonHangViewModel
    {
        public string TenTruyen { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; }
        public float Gia { get; set; }
        public float ThanhTien { get; set; }
        public Nullable<System.DateTime> NgayDat { get; set; }
        public byte? TrangThai { get; set; }
    }
}