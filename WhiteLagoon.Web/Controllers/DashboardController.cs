using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModel;

namespace WhiteLagoon.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Dashboard/GetTotalBookingRadialChartData
        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
           
            return Json(await _dashboardService.GetTotalBookingRadialChartData());
        }

        public async Task<IActionResult> GetRegistereUserChartData()
        {
            return Json(await _dashboardService.GetRegistereUserChartData());
        }


        public async Task<IActionResult> GetRevenueChartData()
        {
            return Json(await _dashboardService.GetRevenueChartData());
        }


        public async Task<IActionResult> GetBookingPieChartData()
        {

            return Json(await _dashboardService.GetBookingPieChartData());
        }


        public async Task<IActionResult> GetMemberAndBookingLineChartData()
        {

            return Json(await _dashboardService.GetMemberAndBookingLineChartData());

        }


    }
}
