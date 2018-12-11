using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Airline.Models;

namespace Airline.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
