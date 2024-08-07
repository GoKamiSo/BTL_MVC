using COMICSHOP.Models;
using COMICSHOP.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COMICSHOP.Controllers
{
    public class ThanhtoanController : Controller
    {
        private truyentranhEntities _context = new truyentranhEntities();

        // GET: Thanhtoan
        public ActionResult Index()
        {
            var gioHang = Session["Cart"] as List<GioHang> ?? new List<GioHang>();

            var viewModel = new Thanhtoan
            {
                Donhang = new Donhang(),
                GioHang = gioHang
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Themdonhang(Thanhtoan model)
        {
            int iddonhang = _context.DonHangs.Count() + 1;

            int accountId = 0;
            if (Session["AccountId"] != null)
            {
                int.TryParse(Session["AccountId"].ToString(), out accountId);
            }

            var newOrder = new DonHang
            {
                IdDonHang = iddonhang,
                TenKhach = model.Donhang.TenKhach,
                DiaChi = model.Donhang.DiaChi,
                SoDienThoai = model.Donhang.SoDienThoai,
                NgayDat = DateTime.Now,
                TongTien = model.GioHang.Sum(ci => ci.ThanhTien),
                AccountID = accountId,
                TrangThai = 0
            };
            _context.DonHangs.Add(newOrder);


            foreach (var item in model.GioHang)
            {
                int maxChiTietDonHangId = _context.ChiTietDonHangs.Count() + 1;

                // Retrieve the current stock for the comic
                var truyen = _context.Truyens.FirstOrDefault(t => t.MaTruyen == item.MaTruyen);
                if (truyen != null)
                {
                    // Check if there is enough stock
                    if (truyen.SoLuong >= item.SoLuongMua)
                    {
                        int maxChiDonHangId = _context.ChiTietDonHangs.Count() + 1;
                        var chiTietDonHang = new ChiTietDonHang
                        {
                            IdChiTietDonHang = maxChiTietDonHangId,
                            MaTruyen = item.MaTruyen,
                            DonGia = item.Gia,
                            SoLuong = item.SoLuongMua,
                            IdDonHang = newOrder.IdDonHang
                        };
                        _context.ChiTietDonHangs.Add(chiTietDonHang);
                        

                        // Update the stock quantity
                        truyen.SoLuong -= item.SoLuongMua;
                        _context.Entry(truyen).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        // Handle insufficient stock case (optional: e.g., return an error or notification)
                        ModelState.AddModelError("", $"Insufficient stock for {item.MaTruyen}");
                        return View(model); // or handle as needed
                    }
                }
            }

            
            Session["Cart"] = null;

            return RedirectToAction("Index", "Trangchu");
        }
    }
}
