using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMICSHOP.Areas.Admin.Data
{
    public class SanPhamVM
    {
        public string TenTruyen { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuongMua { get; set; }
        public double DonGia { get; set; }
        public double ThanhTien { get; set; }
    }
}