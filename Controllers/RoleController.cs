using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Core;

namespace partner_aluro.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = Constants.Policies.RequireAdmin)] //Policy ="RequireManager"
        public IActionResult Manager()
        {
            return View();
        }

        //[Authorize(Policy = "RequireAdmin")]
        //[Authorize(Policy = "RequireAdmin")]
        //Authorize(Roles="Administrator,Manager")]
        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
        public IActionResult Admin()
        {
            return View();
        }
    }
}
