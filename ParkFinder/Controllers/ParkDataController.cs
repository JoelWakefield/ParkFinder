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
        private readonly ApiHelper _apiHelper;

        public ParkDataController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        
        public async Task<IActionResult> Index(string search)
        {
            ViewBag.Parks = await _apiHelper.Get(search);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
