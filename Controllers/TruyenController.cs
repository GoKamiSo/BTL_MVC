using COMICSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COMICSHOP.Controllers
{
    public class TruyenController : Controller
    {
        private truyentranhEntities _dbContext = new truyentranhEntities();
        public ActionResult ChitietSP(int id)
        {
            var book = _dbContext.Truyens.FirstOrDefault(x => x.MaTruyen == id);
            var theLoai = _dbContext.TheLoais.FirstOrDefault(tl => tl.MaTL == id)?.TenTL; // Lấy tên thể loại
            ViewBag.TheLoai = theLoai;
            return View(book);
        }
        public ActionResult SPTT(int id)
        {
            var book = _dbContext.Truyens.FirstOrDefault(x => x.MaTruyen == id );
            var theLoai = _dbContext.TheLoais.FirstOrDefault(tl => tl.MaTL == id)?.TenTL; // Lấy tên thể loại
            ViewBag.TheLoai = theLoai;
            var relatedBooks = _dbContext.Truyens
                .Where(x => x.MaTL == book.MaTL && x.SoLuong > 0)
                .Take(4) // Lấy số lượng sách liên quan tối đa
                .ToList();
            return PartialView(relatedBooks);
        }
    }
}