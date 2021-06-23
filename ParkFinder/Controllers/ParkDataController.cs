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
        private readonly ApiHelper _apiHelper;

        private readonly string ApiUrl = "https://seriouslyfundata.azurewebsites.net/api/parks";

        public ParkDataController(ILogger<ParkDataController> logger, ApiHelper apiHelper)
        {
            _logger = logger;
            _apiHelper = apiHelper;
        }
        
        public async Task<IActionResult> Index(string search)
        {
            //  Get data
            List<Park> parks = await _apiHelper.GetCache(ApiUrl);

            //  filter out results
            if (search != null)
            {
                ViewBag.Parks = parks.Where(p =>
                {
                    return p.ParkName.Contains(search) || p.Description.Contains(search);
                }).ToList();
            }
            else
            {
                ViewBag.Parks = parks;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
