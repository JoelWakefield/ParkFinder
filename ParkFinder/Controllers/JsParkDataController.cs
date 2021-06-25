using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
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
            var data = await _apiHelper.Get();
            return View(data);
        }

        [HttpGet]
        [Route("jsparkdata/getparkdata/{search}")]
        public async Task<JsonResult> GetParkData(string search)
        {
            search = search == "undefined" ? null : search;
            var data = await _apiHelper.Get(search);
            return Json(data);
        } 
    }
}
