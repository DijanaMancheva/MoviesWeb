using Microsoft.AspNetCore.Mvc;

namespace MoviesWeb.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
