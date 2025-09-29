using Microsoft.AspNetCore.Mvc;

namespace FastFood.UI.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
