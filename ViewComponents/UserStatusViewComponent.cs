using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Models;
using partner_aluro.ViewModels;

namespace partner_aluro.ViewComponents
{
    public class UserStatusViewComponent : ViewComponent
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserStatusViewComponent(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            UserStatusModel model = new UserStatusModel();
            model.User = _userManager.GetUserAsync(Request.HttpContext.User).Result;

            return View(model);
        }
    }
}
