using Microsoft.AspNetCore.Mvc;

namespace SurfsUp.Controllers
{
    public class RentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
