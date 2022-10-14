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

        public CategoryController(ICategoryService categoryDB, IUnitOfWorkCategory iUnitOfWorkCategory, IProfildzialalnosciService profildzialalnosciService)
        {
            _categoryService = categoryDB;
            _iUnitOfWorkCategory = iUnitOfWorkCategory;
            _profildzialalnosciService = profildzialalnosciService;
        }

        public IActionResult Index()
        {
            return View();
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

            ViewData["CategoryId"] = id;
            TempData["CategoryId"] = id;
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

        //[HttpGet]
        //public IActionResult ListAll()
        //{
        //    var categories = _categoryService.List();
        //    return View(categories);
        //}


        [HttpGet]
        public IActionResult List()
        {
            //List<Category> kategorie = new List<Category>();
            //kategorie.Add(new Category() { Name = "K1", Description = "K1Opis" });
            //kategorie.Add(new Category() { Name = "K2", Description = "K2Opis" });

            var categories = _categoryService.List();

            return View(categories);
        }


        //TUTAJ WYSWIETLAM STRONE PODSTAWOWĄ DLA WYSWIETLENIA PRODUKTOW Z ID KATEGORIA szukanaNazwa
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Lista(string szukanaNazwa) //Link do wyswietlania po wyborze kategorii
        {
            Core.Constants.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); //Pobierz uzytkownika
            Core.Constants.Rabat = _profildzialalnosciService.GetRabat(Core.Constants.UserId);

            List<Product> pro = _categoryService.ListProductInCategory(szukanaNazwa);


            var produkty2 = pro.ToList();

            var categoryPage = new MvcBreadcrumbNode("Kategoria", "Home", szukanaNazwa);
            ViewData["BreadcrumbNode"] = categoryPage;
            ViewData["Title"] = szukanaNazwa;


            if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
            {
                List<Product> produkty = (List<Product>)szukanie(szukanaNazwa);

                foreach (var produkt in produkty2)
                {

                    produkt.CenaProduktuDlaUzytkownika = produkt.CenaProduktu * (1 - (Core.Constants.Rabat / 100));
                    produkty.Add(produkt);


                    //TUTAJ DODAC ZNIZKE DO PRODUKTU zeby widok wyswietlam oblizona cene WROC TUUUU
                }
                return View(produkty);
            }
            else
            {
                List<Product> produkty = _categoryService.ListProductCategoryAll();
                foreach(var produkt in produkty)
                {
                    produkt.CenaProduktuDlaUzytkownika = produkt.CenaProduktu * (1 - (Core.Constants.Rabat / 100));
                }
                return View(produkty);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Lista2(string szukanaNazwa) //Link do wyswietlania po wyborze kategorii TO JEST TYLKO KONTENER
        {

            if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
            {
                IList<Product> produkty = szukanie(szukanaNazwa);
                foreach (var produkt in produkty)
                {
                    produkt.CenaProduktuDlaUzytkownika = produkt.CenaProduktu * (1 - (Core.Constants.Rabat / 100));
                }
                return View(produkty);
            }
            else
            {
                return View();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IList<Product>? szukanie(string szukanaNazwa)
        {
            List<Product> WszystkieProdukty = _categoryService.ListProductCategoryAll();

            IList<string> WyszukaneNazwyProdukow = new List<string>(); // tworze liste nazw (puste)

            if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
            {
                szukanaNazwa = szukanaNazwa.ToLower();

                foreach (var produkt in WszystkieProdukty)
                {
                    WyszukaneNazwyProdukow.Add(produkt.Name.ToLower());
                }

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
