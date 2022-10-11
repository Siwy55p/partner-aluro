using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.DAL;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.ViewModels;
using System.Security.Claims;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;


        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UserManager<ApplicationUser> _userManager;
        public CartController(Cart cart, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _cart = cart;
            _context = applicationDbContext;

            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public IActionResult Return(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Index()
        {
            var items = _cart.GetAllCartItems();
            _cart.CartItems = items;

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            CartOrderViewModel vm = new CartOrderViewModel
            {
                Carts = _cart,
                Orders = new Order() { User = applicationUser },
            };

            ViewBag.returnUrl = Request.Headers["Referer"].ToString();


            Adress1rozliczeniowy adresRozliczeniowy = new Adress1rozliczeniowy();
            vm.Orders.User.Adres1 = _context.Adress1rozliczeniowy.Where(a => a.UserID == userId).FirstOrDefault();

            if (vm.Orders.User.Adres1 == null)
            {
                vm.Orders.User.Adres1 = new Adress1rozliczeniowy
                {
                    KodPocztowy = "00000",
                    Miasto = "",
                    Kraj = "",
                    Telefon = "123123123"
                };
            }

            vm.Orders.User.Adres2 = _context.Adress2dostawy.Where(a => a.UserID == userId).FirstOrDefault();
            if (vm.Orders.User.Adres2 == null)
            {
                vm.Orders.User.Adres2 = new Adress2dostawy
                {
                    KodPocztowy = vm.Orders.User.Adres1.KodPocztowy,
                    Miasto = vm.Orders.User.Adres1.Miasto,
                    Kraj = vm.Orders.User.Adres1.Kraj,
                    Telefon = vm.Orders.User.Adres1.Telefon
                };
            }

            return View(vm);
        }

        public IActionResult AddToCart(int id)
        {
            var selectedProduct = GetProductId(id);

            if (selectedProduct != null)
            {
                _cart.AddToCart(selectedProduct, 1);
            }

            return RedirectToAction("Index");
        }
        public IActionResult RemoveFromCart(int id)
        {
            var selectedProduct = GetProductId(id);

            if (selectedProduct != null)
            {
                _cart.RemoveFromCart(selectedProduct);
            }

            return RedirectToAction("Index");
        }
        public IActionResult ReduceQuantity(int id)
        {
            var selectedProduct = GetProductId(id);

            if (selectedProduct != null)
            {
                _cart.ReduceQuantity(selectedProduct);
            }

            return RedirectToAction("Index");
        }
        public IActionResult IncreaseQuantity(int id)
        {
            var selectedProduct = GetProductId(id);

            if (selectedProduct != null)
            {
                _cart.IncreaseQuantity(selectedProduct);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            _cart.ClearCart();

            return RedirectToAction("Index");
        }

        public Product GetProductId(int id)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == id);
        }
    }
}
