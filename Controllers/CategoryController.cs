//using AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using partner_aluro.DAL;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewComponents;
using partner_aluro.ViewModels;
using System.Collections.Generic;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        private readonly ICategoryBD _categoryDB;
        private readonly IUnitOfWorkCategory _iUnitOfWorkCategory;

        public CategoryController(ICategoryBD categoryDB, IUnitOfWorkCategory iUnitOfWorkCategory, ApplicationDbContext applicationDbContext)
        {
            _categoryDB = categoryDB;
            _iUnitOfWorkCategory = iUnitOfWorkCategory;
            _ApplicationDbContext = applicationDbContext; 
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
            var id = _categoryDB.AddSave(category);

            ViewData["CategoryId"] = id;
            TempData["CategoryId"] = id;
            return RedirectToAction("List");

        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var kategoria = _categoryDB.Get(id);
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
            var category = _categoryDB.Get(id);
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _categoryDB.Delete(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult ListAll()
        {
            var categories = _categoryDB.List();
            return View(categories);
        }


        
        [HttpGet]
        public IActionResult List()
        {
            //List<Category> kategorie = new List<Category>();
            //kategorie.Add(new Category() { Name = "K1", Description = "K1Opis" });
            //kategorie.Add(new Category() { Name = "K2", Description = "K2Opis" });

            var categories = _categoryDB.List();

            return View(categories);
        }


        //TUTAJ WYSWIETLAM STRONE PODSTAWOWĄ DLA WYSWIETLENIA PRODUKTOW Z ID KATEGORIA szukanaNazwa
        public IActionResult Lista(string szukanaNazwa) //Link do wyswietlania po wyborze kategorii
        {
            List<Product> pro = _ApplicationDbContext.Products.Where(k => k.Category.Name == szukanaNazwa).ToList();

            var produkty2 = pro.ToList();

            if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
            {
                List<Product> produkty = (List<Product>)szukanie(szukanaNazwa);

                foreach (var produkt in produkty2)
                {
                    produkty.Add(produkt);
                }
                return View(produkty);
            }
            else
            {
                return View();
            }
        }

        public IActionResult Lista2(string szukanaNazwa) //Link do wyswietlania po wyborze kategorii TO JEST TYLKO KONTENER
        {

            if (szukanaNazwa != null && szukanaNazwa.Length >= 1)
            {
                IList<Product> produkty = szukanie(szukanaNazwa);

                return View(produkty);
            }
            else
            {
                return View();
            }

        }

        public IList<Product>? szukanie(string szukanaNazwa)
        {
            List<Product> WszystkieProdukty = _ApplicationDbContext.Products.ToList();

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
