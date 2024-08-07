using COMICSHOP.Areas.Admin.Data;
using COMICSHOP.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace COMICSHOP.Areas.Admin.Controllers
{
    public class QLDonHangController : Controller
    {
        // GET: Admin/QLDonHang
        private truyentranhEntities _context = new truyentranhEntities();

        public ActionResult DanhSach(int? page, int pageSize = 10)
        {
            var items = (from dh in _context.DonHangs.OrderByDescending(x => x.NgayDat)
                         select new DonHangDisplayVM()
                         {
                             IdDonHang = dh.IdDonHang,
                             TenKhach = dh.TenKhach,
                             SoDienThoai = dh.SoDienThoai,
                             DiaChi = dh.DiaChi,
                             NgayDat = dh.NgayDat,
                             TrangThai = dh.TrangThai
                         }).ToPagedList(page ?? 1, pageSize);

            return View(items);
        }

        public ActionResult ChiTietDonHang(int id)
        {
            var donHang = (from dh in _context.DonHangs
                           where dh.IdDonHang == id
                           select new CTDHDisplayVM
                           {
                               IdDonHang = dh.IdDonHang,
                               TenKhach = dh.TenKhach,
                               SoDienThoai= dh.SoDienThoai,
                               DiaChi = dh.DiaChi,
                               NgayDat = dh.NgayDat,
                               TrangThai = dh.TrangThai,
                               TongTien = dh.TongTien,
                               SanPhams = (from ctdh in _context.ChiTietDonHangs
                                           join tr in _context.Truyens on ctdh.MaTruyen equals tr.MaTruyen
                                           where ctdh.IdDonHang == dh.IdDonHang
                                           select new SanPhamVM
                                           {
                                               TenTruyen = tr.TenTruyen,
                                               HinhAnh = tr.HinhAnh,
                                               SoLuongMua = ctdh.SoLuong,
                                               DonGia = ctdh.DonGia,
                                               ThanhTien = ctdh.ThanhTien
                                           }).ToList()
                           }).FirstOrDefault();

            var danhSachTT = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Chờ xác nhận" },
                new SelectListItem { Value = "1", Text = "Đã xác nhận" },
                new SelectListItem { Value = "2", Text = "Hàng đang chuẩn bị" },
                new SelectListItem { Value = "3", Text = "Đang giao hàng" },
                new SelectListItem { Value = "4", Text = "Giao hàng thành công" }
            };

            ViewBag.TrangThaiParam = new SelectList(danhSachTT, "Value", "Text");
            return View(donHang);
        }

        [HttpPost]
        public ActionResult ChuyenTrangThai(TrangThaiVM tt)
        {
            var item = _context.DonHangs.Where(x => x.IdDonHang == tt.IdDonHang).FirstOrDefault();
            if (item != null)
            {
                item.TrangThai = tt.TrangThai;
                _context.SaveChanges();
            }
            return RedirectToAction("ChiTietDonHang", new { id = tt.IdDonHang });
        }
    }
}
