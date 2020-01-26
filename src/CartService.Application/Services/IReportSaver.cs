using CartService.Application.DTO.Reports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Services
{
    public interface IReportSaver
    {
        Task SaveShoppingCartReport(string fileName, ShoppingCartsReportDTO data);
    }
}
