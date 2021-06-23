using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkFinder.Models;

namespace ParkFinder.Controllers
{
    public class ParkDataController : Controller
    {
        private readonly ILogger<ParkDataController> _logger;

        public ParkDataController(ILogger<ParkDataController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var parameters = Request.Query;

            ViewBag.Parks = await ApiHelper.GetData("https://seriouslyfundata.azurewebsites.net/api/parks");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
