﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        private readonly IUnitOfWorkAdress1rozliczeniowy _unitOfWorkAdress1rozliczeniowy;
        private readonly IUnitOfWorkAdress2dostawy _unitOfWorkAdress2dostawy;

        private readonly ApplicationDbContext _context;
        private readonly Cart _cart;

        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, Cart cart, UserManager<ApplicationUser> userManager, IOrderService orderService, IUnitOfWork unitOfWork, IUnitOfWorkAdress1rozliczeniowy unitOfWorkAdress1rozliczeniowy, IUnitOfWorkAdress2dostawy unitOfWorkAdress2dostawy)
        {
            _context = context;
            _cart = cart;

            _orderService = orderService;
            _userManager = userManager;

            _unitOfWork = unitOfWork;

            _unitOfWorkAdress1rozliczeniowy = unitOfWorkAdress1rozliczeniowy;
            _unitOfWorkAdress2dostawy = unitOfWorkAdress2dostawy;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(CartOrderViewModel orderCart) //ZAPIS BARDZO WAZNA FUNKCJA
        {
            var cartItems = _cart.GetAllCartItems();
            _cart.CartItems = cartItems;

            if (_cart.CartItems.Count == 0 && _cart != null)
            {
                ModelState.AddModelError("", "Koszyk jest pusty, proszę dodać pierwszy produkt.");
            }

            var user = _unitOfWork.User.GetUser(orderCart.Orders.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.Imie = orderCart.Orders.User.Imie;
            user.Nazwisko = orderCart.Orders.User.Nazwisko;
            user.Email = orderCart.Orders.User.Email;

            _unitOfWork.User.UpdateUser(user); // Zapis 

            //pobierz adres z formularza
            Adress1rozliczeniowy nowyAdres1rozliczeniowy = orderCart.Orders.User.Adres1;
            nowyAdres1rozliczeniowy.UserID = orderCart.Orders.User.Id;
            Adress2dostawy nowyAdres2dostawy = orderCart.Orders.User.Adres2;
            nowyAdres2dostawy.UserID = orderCart.Orders.User.Id;

            ModelState.Remove("Orders.UserID");
            ModelState.Remove("Carts");
            ModelState.Remove("Orders.User.Adres2.UserID");
            ModelState.Remove("Orders.User.Adres2.ApplicationUser");
            if (ModelState.IsValid)
            {
                _unitOfWorkAdress1rozliczeniowy.adress1Rozliczeniowy.Update(nowyAdres1rozliczeniowy);
                _unitOfWorkAdress2dostawy.adress2dostawy.Update(nowyAdres2dostawy);

                Order order = new Order();
                order.Komentarz = orderCart.Orders.Komentarz;
                order.MessageToOrder = orderCart.Orders.MessageToOrder;

                orderCart.Orders = order;

                CreateOrder(orderCart.Orders);
                _cart.ClearCart();

                return View("CheckoutComplete", orderCart.Orders);
            }
            else
            {
                var items = _cart.GetAllCartItems();
                _cart.CartItems = items;
                orderCart.Carts = _cart;
                return View(orderCart);

            }
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
            _orderService.Add(order);
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}")]
        public IActionResult ListaZamowien() // To jest widok listy zamowien w panelu dashoboards
        {
            List<Order> orders = _orderService.ListOrdersAll();

            return View(orders);
        }


        [HttpGet]
        public IActionResult Detail(int id)
        {
            if(id == 0)
            {
                return RedirectToAction("ListaZamowien");
            }
            var orderItems = _orderService.List(id);
            var order = _orderService.GetOrder(id);
            order.OrderItems = orderItems;


            ViewBag.StanyZamowienia = GetStanyZamowienia();

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




        private List<SelectListItem> GetStanyZamowienia()
        {
            var lstStanZamowien = new List<SelectListItem>();

            foreach (StanZamowienia suit in (StanZamowienia[])Enum.GetValues(typeof(StanZamowienia)))
            {
                var dmyItemA = new SelectListItem()
                {
                    Value = suit.ToString(),
                    Text = suit.ToString()
                };
                lstStanZamowien.Insert(0, dmyItemA);
            }

            return lstStanZamowien;
        }
    }
}
