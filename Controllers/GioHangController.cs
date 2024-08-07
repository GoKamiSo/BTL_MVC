using COMICSHOP.Models;
using COMICSHOP.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COMICSHOP.Controllers
{
    public class GioHangController : Controller
    {
        private truyentranhEntities _context = new truyentranhEntities();
        [HttpPost]
        public JsonResult ThemVaoGioHang(GioHang gioHang)
        {
            var cart = Session["Cart"] as List<GioHang> ?? new List<GioHang>();

            var product = _context.Truyens.FirstOrDefault(t => t.MaTruyen == gioHang.MaTruyen);

            if (gioHang.SoLuongMua < 1)
            {
                return Json(new { success = false, message = "Số lượng tối thiểu là 1." });
            }

            if (product.SoLuong < gioHang.SoLuongMua)
            {
                return Json(new { success = false, message = "Số lượng vượt quá số lượng hiện có." });
            }

            var existingItem = cart.FirstOrDefault(x => x.MaTruyen == gioHang.MaTruyen);
            if (existingItem != null)
            {
                return Json(new { success = false, message = "Sản phẩm này đã có trong giỏ." });
            }
            else
            {
                var newItem = new GioHang
                {
                   MaTruyen = gioHang.MaTruyen,
                   HinhAnh = gioHang.HinhAnh,
                   TenTruyen = gioHang.TenTruyen,
                   Gia = gioHang.Gia,
                   SoLuongMua = gioHang.SoLuongMua,
                   AccountId = gioHang.AccountId
                };
                cart.Add(newItem);
                Session["Cart"] = cart;
                return Json(new { success = true });
            }        
        }
        public ActionResult GioHang()
        {
            var cart = Session["Cart"] as List<GioHang> ?? new List<GioHang>();
            return View(cart);
        }
        [HttpPost]
        public JsonResult CapNhatSoLuong(int maTruyen, int soLuong)
        {
            var cart = Session["Cart"] as List<GioHang> ?? new List<GioHang>();

            var item = cart.FirstOrDefault(x => x.MaTruyen == maTruyen);
            if (item != null)
            {
                var product = _context.Truyens.FirstOrDefault(t => t.MaTruyen == maTruyen);

                if (product != null && product.SoLuong < soLuong)
                {
                    return Json(new { success = false, message = "Số lượng không đủ đáp ứng." });
                }

                item.SoLuongMua = soLuong;

                // Cập nhật tổng tiền giỏ hàng
                var tongTien = cart.Sum(x => x.ThanhTien);

                return Json(new { success = true, thanhTien = item.ThanhTien, tongTien = tongTien });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult XoaHang(int maTruyen)
        {
            var cart = Session["Cart"] as List<GioHang>;
            var itemToRemove = cart.FirstOrDefault(x => x.MaTruyen == maTruyen);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                Session["Cart"] = cart;

                var tongTien = cart.Sum(x => x.ThanhTien);

                return Json(new { success = true, tongTien = tongTien });
            }
            else
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
            }
        }
    }
}