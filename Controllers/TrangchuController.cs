using COMICSHOP.Models;
using COMICSHOP.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COMICSHOP.Controllers
{
    public class TrangchuController : Controller
    {
        private truyentranhEntities _dbContext = new truyentranhEntities();

        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            List<Truyen> lst = _dbContext.Truyens.Where(u => u.SoLuong > 0).ToList();
            return View(lst.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult PartialTheloai()
        {
            var lstTL = _dbContext.TheLoais.Take(10).ToList();
            return PartialView(lstTL);
        }

        public ActionResult GetBooksByTopic(int id, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var lstTruyens = _dbContext.Truyens.Where(x => x.MaTL == id).OrderByDescending(p => p.Gia).ToList();
            var theLoai = _dbContext.TheLoais.FirstOrDefault(tl => tl.MaTL == id)?.TenTL;
            ViewBag.TheLoai = theLoai;
            return View(lstTruyens.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult LSGiaoDich()
        {
            var tenKhach = Session["Fullname"] != null ? Session["Fullname"].ToString() : "";
            var lst = from chiTiet in _dbContext.ChiTietDonHangs
                      join donHang in _dbContext.DonHangs
                      on chiTiet.IdDonHang equals donHang.IdDonHang
                      join truyen in _dbContext.Truyens
                      on chiTiet.MaTruyen equals truyen.MaTruyen
                      where donHang.TenKhach == tenKhach
                      select new ChiTietDonHangViewModel
                      {
                          TenTruyen = truyen.TenTruyen,
                          SoLuong = chiTiet.SoLuong,
                          HinhAnh = truyen.HinhAnh,
                          Gia = (float)truyen.Gia,
                          ThanhTien = (float)chiTiet.ThanhTien,
                          NgayDat = donHang.NgayDat,
                          TrangThai = (byte)donHang.TrangThai
                      };
            return View(lst.ToList());
        }

        public ActionResult Search(string query)
        {
            var result = _dbContext.Truyens
                           .Where(t => t.TenTruyen.Contains(query) || t.TacGia.Contains(query))
                           .ToList();
            return View("Index", result);
        }
    }
}
