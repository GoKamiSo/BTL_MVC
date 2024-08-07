using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace COMICSHOP.Areas.Admin.Data
{
    public class TruyenDisplayVM
    {
        public int MaTruyen { get; set; }
        [DisplayName("Tên truyện")]
        public string TenTruyen { get; set; }
        [DisplayName("Ảnh")]
        public string AnhBia { get; set; }
        [DisplayName("Giá")]
        public double? Gia { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? NgayXuatBan { get; set; }
        [DisplayName("Số lượng")]
        public int? SoLuong { get; set; }
        [DisplayName("Tên thể loại")]
        public string TenTheLoai { get; set; }
    }
}