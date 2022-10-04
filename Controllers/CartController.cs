using Microsoft.AspNetCore.Mvc;
using partner_aluro.DAL;
using partner_aluro.Models;
using partner_aluro.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace partner_aluro.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;
        
        public CartController(Cart cart, ApplicationDbContext applicationDbContext)
        {
            _cart = cart;
            _context = applicationDbContext;
        }

        public IActionResult Index()
        {
            var items = _cart.GetAllCartItems();
            _cart.CartItems = items;
                        

            CartOrderViewModel vm = new CartOrderViewModel
            {
                Carts = _cart,
            };



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
