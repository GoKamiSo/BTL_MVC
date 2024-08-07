using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace COMICSHOP.Areas.Admin.Data
{
    public class DashboardViewModel
    {
        public int? TotalComicsInStock { get; set; }
        public int? TotalRevenueThisWeek { get; set; }
        public IPagedList<Order> IncompleteOrders { get; set; }
        public List<Comic> OutOfStockComics { get; set; }
        public int? OrdersToday { get; set; }
    }

    public class Order
    {
        public int? Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int? TotalPrice { get; set; }
        public int? Status { get; set; }
    }

    public class Comic
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? Quantity { get; set; }
    }
}