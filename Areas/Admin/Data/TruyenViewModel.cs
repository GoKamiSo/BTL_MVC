using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COMICSHOP.Areas.Admin.Data
{
    public class TruyenViewModel
    {
        public int MaTruyen { get; set; }
        public string TenTruyen { get; set; }
        public string HinhAnh { get; set; }
        public string TacGia { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? NgayXuatBan { get; set; }
        public int SoLuong { get; set; }
        public double? Gia { get; set; }
        public string TomTat { get; set; }
        public int MaTL { get; set; }
    }
}