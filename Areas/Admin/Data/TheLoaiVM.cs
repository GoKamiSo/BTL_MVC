using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace COMICSHOP.Areas.Admin.Data
{
    public class TheLoaiVM
    {
        [DisplayName("Mã thể loại")]
        public int MaTL { get; set; }
        [DisplayName("Tên thể loại")]
        public string TenTL { get; set; }
    }
}