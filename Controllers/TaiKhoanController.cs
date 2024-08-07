using COMICSHOP.Models;
using COMICSHOP.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace COMICSHOP.Controllers
{
    public class TaiKhoanController : Controller
    {
        // GET: TaiKhoan
        private truyentranhEntities _context = new truyentranhEntities();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(TkDisplayVM tk)
        {
            if (ModelState.IsValid)
            {
                var taikhoan = _context.TaiKhoans.FirstOrDefault(t => t.Username == tk.Username && t.Password == tk.Password && t.Role == "User");

                if (taikhoan != null)
                {
                    Session["Fullname"] = taikhoan.Fullname;
                    Session["sdt"] = taikhoan.Sdt;
                    Session["AccountId"] = taikhoan.AccountID;
                    return RedirectToAction("Index", "Trangchu");
                                       
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng!");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(TaiKhoan tk)
        {
            if (ModelState.IsValid)
            {
                var checktaikhoan = _context.TaiKhoans.FirstOrDefault(t => t.Username == tk.Username);
                var usernameRegex = new Regex(@"^[a-zA-Z0-9]+$");
                if (checktaikhoan != null)
                {
                    ModelState.AddModelError("", "Tài khoản này đã được sử dụng!");
                }
                else if (!usernameRegex.IsMatch(tk.Username))
                {
                    ModelState.AddModelError("", "Tên tài khoản chỉ được chứa chữ cái và số!");
                }
                else if (tk.Password.Length < 5 || tk.Password.Length > 12)
                {
                    ModelState.AddModelError("", "Mật khẩu phải có độ dài từ 5-12 ký tự!");
                }
                else
                {
                    var taikhoan = new TaiKhoan();
                    taikhoan.Username = tk.Username;
                    taikhoan.Password = tk.Password;
                    taikhoan.Fullname = tk.Fullname;
                    taikhoan.Sdt = tk.Sdt;
                    taikhoan.Role = "User";
                    _context.TaiKhoans.Add(taikhoan);
                    _context.SaveChanges();
                    return RedirectToAction("Login", "TaiKhoan");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["Fullname"] = null;
            Session["sdt"] = null;
            Session["AccountId"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "TaiKhoan");
        }
    }
}