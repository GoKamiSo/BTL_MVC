using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COMICSHOP.Models.ViewModel
{
    public class TkDisplayVM
    {
        public int AccountID { get; set; }
        [DisplayName("Tài khoản")]
        public string Username { get; set; }

        [DisplayName("Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Họ tên")]
        public string Fullname { get; set; }

        [DisplayName("Số điện thoại")]
        public string Sdt { get; set; }
        public string Role { get; set; }
    }
}