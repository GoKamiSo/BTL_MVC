using COMICSHOP.Areas.Admin.Data;
using COMICSHOP.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COMICSHOP.Areas.Admin.Controllers
{
    public class QLTruyenController : Controller
    {
        // GET: Admin/QLTruyen
        private truyentranhEntities _context = new truyentranhEntities();
        public ActionResult DanhSach(int? page)
        {
            int pageSize = 10; // You can set the page size as needed
            int pageNumber = (page ?? 1);

            var danhSachTruyen = (from tr in _context.Truyens
                                  join tl in _context.TheLoais on tr.MaTL equals tl.MaTL into joined
                                  from tl in joined.DefaultIfEmpty()
                                  select new TruyenDisplayVM()
                                  {
                                      MaTruyen = tr.MaTruyen,
                                      TenTruyen = tr.TenTruyen,
                                      Gia = tr.Gia,
                                      NgayXuatBan = tr.NgayXuatBan,
                                      AnhBia = tr.HinhAnh,
                                      TenTheLoai = tl != null ? tl.TenTL : "dữ liệu trống",
                                      SoLuong = tr.SoLuong,
                                  }).ToList();

            return View(danhSachTruyen.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            var danhSachTL = _context.TheLoais.Select(t => new SelectListItem
            {
                Value = t.MaTL.ToString(),
                Text = t.TenTL
            }).ToList();

            ViewBag.MaTLParam = new SelectList(danhSachTL, "Value", "Text");
            return View();
        }
        [HttpPost]
        public JsonResult CheckFileExists(string fileName)
        {
            var filePath = Path.Combine(Server.MapPath("~/Images/"), fileName);
            bool fileExists = System.IO.File.Exists(filePath);
            return Json(new { exists = fileExists });
        }
        [HttpPost]
        public JsonResult Create(TruyenViewModel formdata, HttpPostedFileBase fileUpload)
        {
            int countTruyen = _context.Truyens.Count();
            countTruyen = (countTruyen == 0) ? 1 : countTruyen + 1;

            var itemNew = new Truyen();

            itemNew.MaTruyen = countTruyen;
            itemNew.TenTruyen = formdata.TenTruyen;
            itemNew.Gia = formdata.Gia;
            itemNew.TacGia = formdata.TacGia;
            itemNew.TomTat = formdata.TomTat;
            itemNew.NgayXuatBan = formdata.NgayXuatBan;
            itemNew.HinhAnh = formdata.HinhAnh;
            itemNew.MaTL = formdata.MaTL;
            itemNew.SoLuong = formdata.SoLuong;

                var fileName = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/"), fileName);

                fileUpload.SaveAs(path);
                itemNew.HinhAnh = fileName;               
            

            _context.Truyens.Add(itemNew);
            _context.SaveChanges();

            return Json(new { success = true });
        }
        
        [HttpGet]
        public ActionResult EditTruyen(int id)
        {
            var item = _context.Truyens.Where(x => x.MaTruyen == id).Select(x => new TruyenViewModel()
            {
                MaTruyen = x.MaTruyen,
                TenTruyen = x.TenTruyen,
                HinhAnh = x.HinhAnh,
                TacGia = x.TacGia,
                NgayXuatBan = x.NgayXuatBan,
                SoLuong = x.SoLuong ?? 0,
                Gia = x.Gia ?? 0,
                TomTat = x.TomTat
            }).FirstOrDefault();
            var danhSachTL = _context.TheLoais.Select(t => new SelectListItem
            {
                Value = t.MaTL.ToString(),
                Text = t.TenTL
            }).ToList();

            ViewBag.MaTLParam = new SelectList(danhSachTL, "Value", "Text");
            return View(item);
        }
        [HttpPost]
        public JsonResult EditTruyen(TruyenViewModel formdata, HttpPostedFileBase fileUpload)
        {
            var item = _context.Truyens.Where(x => x.MaTruyen == formdata.MaTruyen).FirstOrDefault();
            item.TenTruyen = formdata.TenTruyen;
            item.TacGia = formdata.TacGia;
            item.NgayXuatBan = formdata.NgayXuatBan;
            item.SoLuong = formdata.SoLuong;
            item.Gia = formdata.Gia;
            item.TomTat = formdata.TomTat;
            item.MaTL = formdata.MaTL;
            if (fileUpload != null)
            {
                var fileName = System.IO.Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/"), fileName);

                if (!string.IsNullOrEmpty(item.HinhAnh))
                {
                    var oldImagePath = Path.Combine(Server.MapPath("~/Images/"), item.HinhAnh);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                fileUpload.SaveAs(path);
                item.HinhAnh = fileName;
            }
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult DeleteTruyen(int id)
        {
            var item = _context.Truyens.Where(x => x.MaTruyen == id).Select(x => new TruyenViewModel()
            {
                MaTruyen = x.MaTruyen,
                TenTruyen = x.TenTruyen,
                HinhAnh = x.HinhAnh
            }).FirstOrDefault();

            return View(item);
        }

        [HttpPost]
        public ActionResult ConfirmDeleteTruyen(TruyenViewModel formData)
        {
            var item = _context.Truyens.Where(x => x.MaTruyen == formData.MaTruyen).FirstOrDefault();
            
            if (item == null)
            {
                return RedirectToAction("DanhSach", "QLTruyen");
            }
            else
            {

                var referenceMessages = new List<string>();

                // Check for foreign key references
                if (_context.ChiTietDonHangs.Any(x => x.MaTruyen == formData.MaTruyen))
                {
                    referenceMessages.Add("Chi tiết đơn hàng");
                }
                if (_context.TheLoais.Any(x => x.MaTL == formData.MaTL))
                {
                    referenceMessages.Add("Thể loại");
                }

                // Add similar checks for other tables that might reference HangHoa

                if (referenceMessages.Any())
                {
                    TempData["Message"] = $"Không thể xóa vì truyện đang bị ràng buộc với bảng: {string.Join(", ", referenceMessages)}.";
                    return RedirectToAction("DanhSach", "QLTruyen");
                }

                var imagePath = Server.MapPath("~/Images/" + item.HinhAnh);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _context.Truyens.Remove(item);
                _context.SaveChanges();
                TempData["Message"] = "Xóa thành công!";
                return RedirectToAction("DanhSach", "QLTruyen");
            }
            //xoa anh khoi thu muc ~Images/item.AnhBia
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}