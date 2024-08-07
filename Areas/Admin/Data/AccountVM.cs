using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace COMICSHOP.Areas.Admin.Data
{
    public class AccountVM
    {
        public int AccountID { get; set; }
        [Required(ErrorMessage = "Hãy nhập tài khoản!")]
        [DisplayName("Tài khoản")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Hãy nhập mật khẩu!")]
        [DisplayName("Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Fullname")]
        public string Fullname { get; set; }
        [DisplayName("Sdt")]
        public string Sdt { get; set; }
        [DisplayName("Role")]
        public string Role { get; set; }
    }
}