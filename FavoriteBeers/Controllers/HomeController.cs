using Microsoft.AspNetCore.Mvc;
using FavoriteBeers.Models;

namespace FavoriteBeers.Controllers
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
