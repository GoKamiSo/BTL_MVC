using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COMICSHOP.Models.ViewModel
{
    public class Donhang
    {
        public int IdDonHang { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ tên!")]
        public string TenKhach { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại chỉ bắt đầu từ số 0 và phải đủ 10 số!")]
        public string SoDienThoai { get; set; }
        public double? TongTien { get; set; }
        public DateTime? NgayDat { get; set; }
        public int? AccountId { get; set; }
    }
}