using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMICSHOP.Models.ViewModel
{
    public class Thanhtoan
    {
        public Donhang Donhang { get; set; }
        public List<GioHang> GioHang { get; set; }
    }
}