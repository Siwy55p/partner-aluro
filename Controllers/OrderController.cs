
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Core;
using partner_aluro.Core.Repositories;
using partner_aluro.DAL;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewModels;
using System;
using static NuGet.Packaging.PackagingConstants;
using Order = partner_aluro.Models.Order;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;


        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, Cart cart, UserManager<ApplicationUser> userManager, IOrderService orderService, IUnitOfWork unitOfWork)
        {
            _context = context;
            _cart = cart;

            _orderService = orderService;
            _userManager = userManager;

            _unitOfWork = unitOfWork;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var cartItems = _cart.GetAllCartItems();
            _cart.CartItems = cartItems;

            if (_cart.CartItems.Count == 0)
            {
                ModelState.AddModelError("", "Koszyk jest pusty, proszę dodać pierwszy produkt.");
            }

            if (ModelState.IsValid)
            {
                CreateOrder(order);
                _cart.ClearCart();

                return View("CheckoutComplete", order);
            }

            return View(order);
        }


        [HttpPost]
        public IActionResult Checkout2(CartOrderViewModel orderCart)
        {
            var cartItems = _cart.GetAllCartItems();
            _cart.CartItems = cartItems;

            Order order = new Order();
            orderCart.Orders = order;

            if (_cart.CartItems.Count == 0)
            {
                ModelState.AddModelError("", "Koszyk jest pusty, proszę dodać pierwszy produkt.");
            }

            if (ModelState.IsValid)
            {
                CreateOrder(orderCart.Orders);
                _cart.ClearCart();

                return View("CheckoutComplete", orderCart.Orders);
            }

            return View(orderCart.Orders);
        }



        public IActionResult CheckoutComplete(Order order)
        {
            return View(order);
        }


        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;

            var cartItems = _cart.CartItems;

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem()
                {
                    Quantity = item.Quantity,
                    ProductId = item.Product.ProductId,
                    OrderId = order.Id,
                    Cena = (int)(item.Product.CenaProduktu * item.Quantity)
                };

                order.UserID = _userManager.GetUserId(HttpContext.User);

                order.OrderItems.Add(orderItem);
                order.OrderTotal += orderItem.Cena;
                order.StanZamowienia = StanZamowienia.Nowe;
            }
            
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
        public IActionResult ListaZamowien() // To jest widok listy zamowien w panelu dashoboards
        {
            List<ApplicationUser> applicationUsers = _context.Users.ToList();
            List<Order> orders = _context.Orders.ToList();

            OrderListModel vm = new OrderListModel
            {
                Orders = orders,
                Users = applicationUsers,
            };

            return View(vm);
        }

        public IActionResult Detail(int id)
        {
            if(id == 0)
            {
                return RedirectToAction("ListaZamowien");
            }
            var orderItems = _orderService.List(id);
            var order = _orderService.GetOrder(id);
            order.OrderItems = orderItems;


            var adres1 = _orderService.GetUserAdress1(order.UserID);
            var adres2 = _orderService.GetUserAdress2(order.UserID);
            order.User.Adres1 = adres1;
            order.User.Adres2 = adres2;

            return View(order);
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
        [HttpPost]
        public IActionResult ZapiszNotatke(Order order)
        {

            var user = _unitOfWork.User.GetUser(order.User.Id);

            user.NotatkaOsobista = order.User.NotatkaOsobista;

            _unitOfWork.User.UpdateUser(user);

            int id = order.Id;

            //user.NotatkaOsobista = notatka;
            return RedirectToAction("Detail", new {id = id});
        }

    }
}
