using COMICSHOP.Areas.Admin.Data;
using COMICSHOP.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COMICSHOP.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private truyentranhEntities _context = new truyentranhEntities();

        // GET: Admin/Dashboard
        public ActionResult Index(int? page, int pageSize = 10)
        {
            var viewModel = new DashboardViewModel();

            // Tổng số truyện tồn kho
            viewModel.TotalComicsInStock = _context.Truyens.Sum(t => t.SoLuong);

            // Tổng tiền của cột TongTien trong tuần
            var startDate = DateTime.Now.AddDays(-7);
            viewModel.TotalRevenueThisWeek = _context.DonHangs
                .Where(d => d.NgayDat >= startDate && d.NgayDat <= DateTime.Now)
                .Sum(d => (int?)d.TongTien) ?? 0;

            // Đơn hàng chưa hoàn thành (TrangThai != 4)
            var incompleteOrders = _context.DonHangs
                .Where(d => d.TrangThai != 4)
                .Select(d => new Order
                {
                    Id = d.IdDonHang,
                    OrderDate = (DateTime)d.NgayDat,
                    TotalPrice = (int?)d.TongTien,
                    Status = d.TrangThai
                })
                .OrderBy(d => d.Id)
                .ToList();

            viewModel.IncompleteOrders = incompleteOrders.ToPagedList(page ?? 1, pageSize);

            // Truyện có SoLuong = 0
            viewModel.OutOfStockComics = _context.Truyens
                .Where(t => t.SoLuong == 0)
                .Select(t => new Comic
                {
                    Id = t.MaTruyen,
                    Name = t.TenTruyen,
                    Quantity = t.SoLuong
                })
                .ToList();

            // Calculate the start and end dates for today
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            // Số đơn hàng với NgayDat trùng với ngày hiện tại
            viewModel.OrdersToday = _context.DonHangs
                .Count(d => d.NgayDat >= today && d.NgayDat < tomorrow);

            return View(viewModel);
        }
    }
}
