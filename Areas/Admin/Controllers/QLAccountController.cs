using COMICSHOP.Areas.Admin.Data;
using COMICSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace COMICSHOP.Areas.Admin.Controllers
{
    public class QLAccountController : Controller
    {
        private truyentranhEntities _context = new truyentranhEntities();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AccountVM tk)
        {
            if (ModelState.IsValid)
            {
                var taikhoan = _context.TaiKhoans.FirstOrDefault(t => t.Username == tk.Username && t.Password == tk.Password && t.Role == "Admin");

                if (taikhoan != null)
                {
                    Session["Fullname"] = taikhoan.Fullname;
                    Session["Role"] = taikhoan.Role;

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng!");
                }
            }
            return View();
        }
        public ActionResult DanhSachAcc()
        {
            var lstAcc = (from ac in _context.TaiKhoans
                          select new AccountVM()
                          {
                              AccountID = ac.AccountID,
                              Username = ac.Username,
                              Password = ac.Password,
                              Fullname = ac.Fullname,
                              Sdt = ac.Sdt,
                              Role = ac.Role,
                          }).ToList();
            return View(lstAcc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAcc(AccountVM formData)
        {
            if (ModelState.IsValid)
            {
                var newTaiKhoan = new TaiKhoan
                {
                    Username = formData.Username,
                    Password = formData.Password,
                    Fullname = formData.Fullname,
                    Sdt = formData.Sdt,
                    Role = formData.Role,
                };

                _context.TaiKhoans.Add(newTaiKhoan);
                _context.SaveChanges();

                return RedirectToAction("DanhSachAcc");
            }

            return View(formData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAcc(AccountVM formData)
        {
            if (ModelState.IsValid)
            {
                var existingTaiKhoan = _context.TaiKhoans.Find(formData.AccountID);
                if (existingTaiKhoan != null)
                {
                    existingTaiKhoan.Username = formData.Username;
                    existingTaiKhoan.Password = formData.Password;
                    existingTaiKhoan.Fullname = formData.Fullname;
                    existingTaiKhoan.Sdt = formData.Sdt;
                    existingTaiKhoan.Role = formData.Role;

                    _context.SaveChanges();
                }

                return RedirectToAction("DanhSachAcc");
            }

            return View(formData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAcc(int AccountID)
        {
            var taiKhoan = _context.TaiKhoans.Find(AccountID);
            if (taiKhoan != null)
            {
                _context.TaiKhoans.Remove(taiKhoan);
                _context.SaveChanges();
            }

            return RedirectToAction("DanhSachAcc");
        }

        public ActionResult Logout()
        {
            Session["Fullname"] = null;
            Session["Role"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "QLAccount");
        }
    }
}
