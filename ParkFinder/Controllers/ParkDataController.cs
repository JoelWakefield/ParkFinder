using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ParkFinder.Controllers
{
    public class ParkDataController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Parks = await ApiHelper.GetData("https://seriouslyfundata.azurewebsites.net/api/parks");

            return View();
        }
    }
}
