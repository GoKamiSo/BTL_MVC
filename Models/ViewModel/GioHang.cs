using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace COMICSHOP.Models.ViewModel
{
    public class GioHang
    {
        public int MaTruyen { get; set; }
        public string HinhAnh { get; set; }
        [DisplayName("Tên truyện")]
        public string TenTruyen { get; set; }
        [DisplayName("Giá")]
        public float Gia { get; set; }
        [DisplayName("Số lượng")]
        public int SoLuongMua { get; set; }
        [DisplayName("Thành tiền")]
        public float ThanhTien => Gia * SoLuongMua;
        public int? AccountId { get; set; }
    }
}