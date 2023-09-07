using Microsoft.AspNetCore.Mvc;

namespace CRMWebClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProjectView()
        {
            return View();
        }
    }
}
