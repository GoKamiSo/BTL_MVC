using COMICSHOP.Areas.Admin.Data;
using COMICSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COMICSHOP.Areas.Admin.Controllers
{
    public class TheLoaiController : Controller
    {
        // GET: Admin/TheLoai
        private readonly truyentranhEntities _context = new truyentranhEntities();
        public ActionResult DanhSach()
        {
            var danhsachTL = _context.TheLoais.ToList();
            return View(danhsachTL);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TheLoai theLoai)
        {
            int maxMaTL = _context.TheLoais.Any() ? _context.TheLoais.Max(t => t.MaTL) : 0;
            theLoai.MaTL = maxMaTL + 1;

            _context.TheLoais.Add(theLoai);
            _context.SaveChanges();

            return Json(new { success = true, message = "Thể loại đã được thêm thành công." });
        }
        public JsonResult IsTentlExists(string tentl)
        {
            var exists = _context.TheLoais.Any(t => t.TenTL.Equals(tentl, StringComparison.OrdinalIgnoreCase));
            return Json(exists, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditTheLoai(int id)
        {
            var item = _context.TheLoais.Where(x => x.MaTL == id).Select(x => new TheLoaiVM()
            {
                MaTL = x.MaTL,
                TenTL = x.TenTL
            }).FirstOrDefault();
            return View(item);
        }
        [HttpPost]
        public ActionResult EditTheLoai(TheLoaiVM formdata)
        {
            var item = _context.TheLoais.Where(x => x.MaTL == formdata.MaTL).FirstOrDefault();

            if (item == null)
            {
                return RedirectToAction("DanhSach", "TheLoai");
            }

            item.TenTL = formdata.TenTL;

            _context.SaveChanges();

            return Json(new { success = true, message = "Cập nhật thể loại thành công." });
        }
        [HttpGet]
        public ActionResult DeleteTheLoai(int id)
        {
            var item = _context.TheLoais.Where(x => x.MaTL == id).Select(x => new TheLoaiVM()
            {
                MaTL = x.MaTL,
                TenTL = x.TenTL
            }).FirstOrDefault();
            return View(item);
        }
        [HttpPost]
        public ActionResult ConfirmDeleteTheLoai(TheLoaiVM formData)
        {
            var item = _context.TheLoais.Where(x => x.MaTL == formData.MaTL).FirstOrDefault();

            var truyen = _context.Truyens.Where(x => x.MaTL == formData.MaTL).ToList();

            foreach (var tr in truyen)
            {
                tr.MaTL = null;
            }

            _context.TheLoais.Remove(item);

            _context.SaveChanges();

            return RedirectToAction("DanhSach", "TheLoai");
        }
    }
}