using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using System.Security.Claims;

namespace partner_aluro.Controllers
{
    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;
        private readonly IProfildzialalnosciService _profildzialalnosciService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UserManager<ApplicationUser> _userManager;
        public CartController(Cart cart, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IProfildzialalnosciService profildzialalnosciService)
        {
            _cart = cart;
            _context = applicationDbContext;

            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _profildzialalnosciService = profildzialalnosciService;
        }

        [HttpPost]
        public IActionResult Return(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Index()
        {
            Core.Constants.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); //Pobierz uzytkownika
            Core.Constants.Rabat = _profildzialalnosciService.GetRabat(Core.Constants.UserId);

            var products = _cart.GetAllCartItems();
            _cart.CartItems = products;

            foreach(var product in products)
            {
                product.Product.CenaProduktuDlaUzytkownika = product.Product.CenaProduktu * (1 - (Core.Constants.Rabat / 100));
            }


            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            CartOrderViewModel vm = new CartOrderViewModel
            {
                Carts = _cart,
                Orders = new Order() { User = applicationUser },
            };

            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            var returnUrl = Request.Headers["Referer"].ToString();
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

            return Redirect(returnUrl);

            //return View(vm);
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
