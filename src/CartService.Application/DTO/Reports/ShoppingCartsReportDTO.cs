using System;
using System.Collections.Generic;
using System.Text;

namespace CartService.Application.DTO.Reports
{
    public class ShoppingCartsReportDTO
    {
        public int TotalCarts { get; set; }
        public int TotalCartsWithBonuses { get; set; }
        public int TotalCartsExpiredIn10Days { get; set; }
        public int TotalCartsExpiredIn20Days { get; set; }
        public int TotalCartsExpiredIn30Days { get; set; }
        public decimal AverageCheck { get; set; }
    }
}
