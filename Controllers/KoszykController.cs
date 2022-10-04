using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class KoszykController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DodajDoKoszyka(string id)
        {
            return View();
        }
    }
}
