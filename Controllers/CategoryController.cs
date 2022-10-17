//using AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using partner_aluro.Core;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewComponents;
using partner_aluro.ViewModels;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;
using System.Collections.Generic;
using System.Security.Claims;

namespace partner_aluro.Controllers
{

    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager},{Constants.Roles.User}")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWorkCategory _iUnitOfWorkCategory;
        private readonly IProfildzialalnosciService _profildzialalnosciService;

        private readonly ApplicationDbContext _context;

        private readonly Cart _cart;

        public CategoryController(Cart cart, ICategoryService categoryDB, IUnitOfWorkCategory iUnitOfWorkCategory, IProfildzialalnosciService profildzialalnosciService, ApplicationDbContext context)
        {
            _cart = cart;
            _categoryService = categoryDB;
            _iUnitOfWorkCategory = iUnitOfWorkCategory;
            _profildzialalnosciService = profildzialalnosciService;
            _context = context; 
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            //logika zapisania kategorii do bazy.
            var id = _categoryService.AddSave(category);

            return RedirectToAction("List");

        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var kategoria = _categoryService.Get(id);
            return View(kategoria);
        }

        [HttpPost]
        public IActionResult Edit(Category data)
        {
            var kategoria = _iUnitOfWorkCategory.Category.Get(data.CategoryId);

            if (kategoria == null)
            {
                return NotFound();
            }

            kategoria.Name = data.Name;
            kategoria.Description = data.Description;

            _iUnitOfWorkCategory.Category.Update(kategoria);

            return RedirectToAction("List");
        }


        [HttpGet]        
        public IActionResult Details(int id)
        {
            var category = _categoryService.Get(id);
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _categoryService.Delete(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _context.Category.ToListAsync());
        }


        //TUTAJ WYSWIETLAM STRONE PODSTAWOWĄ DLA WYSWIETLENIA PRODUKTOW Z ID KATEGORIA szukanaNazwa
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Lista(string szukanaNazwa) //Link do wyswietlania po wyborze kategorii
        {

            var products = _cart.GetAllCartItems();

            //jesli jest cos w karcie przekaz do zmiennej, pokaz wartosc karty true
            if (products.Count > 0)
            {
                ViewData["Pokaz"] = "show";
            }

            if (szukanaNazwa == null)
            {
                szukanaNazwa = "";

                return View(await _context.Products.ToListAsync());
            }

            //var categoryPage = new MvcBreadcrumbNode("Kategoria", "Home", szukanaNazwa);
            //ViewData["BreadcrumbNode"] = categoryPage;
            //ViewData["Title"] = szukanaNazwa;


            List<Product> produkty2 = await _context.Products.Where(x => x.CategoryNavigation.Name == szukanaNazwa).ToListAsync();

            if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
            {
                List<Product> produkty = (List<Product>)await szukanie(szukanaNazwa);

                foreach (var produkt in produkty2)
                {
                    produkty.Add(produkt);
                }
                return View(produkty);
            }
            else
            {
                return View(produkty2);
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Lista2(string szukanaNazwa) //Link do wyswietlania po wyborze kategorii TO JEST TYLKO KONTENER
        {

            if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
            {
                IList<Product> produkty = await szukanie(szukanaNazwa);

                return View(produkty);
            }
            else
            {
                return View();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IList<Product>>? szukanie(string szukanaNazwa)   // to jest wynik wyszukiwarki 
        {
            List<Product> WszystkieProdukty = await _context.Products.ToListAsync();

            IList<string> WyszukaneNazwyProdukow = await _context.Products.Select(p=> p.Name.ToLower()).ToListAsync(); // tworze liste nazw (puste)

            if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
            {
                szukanaNazwa = szukanaNazwa.ToLower();

                //WyszukaneNazwyProdukow zawiera wszystkie nazwy wszystkich produków

                IList<Product> WyszukaneProduktyLowerName = new List<Product>();


                foreach (string NazwaProduktu in WyszukaneNazwyProdukow)
                {
                    if (NazwaProduktu.Contains(szukanaNazwa)) // jesli jakas nazwa jest w liscie 
                    {
                        WyszukaneProduktyLowerName.Add(WszystkieProdukty.Find(x => x.Name.ToLower() == NazwaProduktu));
                    }
                }

                return WyszukaneProduktyLowerName;
            }
            else
            {
                return null;
            }
        }
    }
}
