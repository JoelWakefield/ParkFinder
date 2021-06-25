using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkFinder.Models;

namespace ParkFinder.Controllers
{
    public class JsParkDataController : Controller
    {
        private readonly ApiHelper _apiHelper;

        public JsParkDataController(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Parks = await _apiHelper.Get();

            return View();
        }

        public async Task<IActionResult> GetParksAsync(string query)
        {
            var data = await _apiHelper.Get(query);

            return Json(data);
        } 
    }
}
