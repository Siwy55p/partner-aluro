using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }
    }
}
