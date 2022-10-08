using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using partner_aluro.Core;
using partner_aluro.DAL;
using partner_aluro.Models;
using partner_aluro.ViewModels;
using System.Configuration;
using System.Diagnostics;

namespace partner_aluro.Controllers
{
    [Authorize]

    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager},{Constants.Roles.User}")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;
        //Kontrolery odzoruwuja widoki , sluza do generowania róznych treści
        public HomeController(ApplicationDbContext context,ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //logika zalogowania
            if (User.Identity.IsAuthenticated)
            {
                // PrawidlowaStrona();
                //return View();

                var cat = _context.Category.ToList();
                
                    //zawsze trzeba pobrac dane i wrzucic do widoku
                    var kategorie = _context.Category.ToList();

                    //pobieramu produkty
                    var nowosci = _context.Products.Where(a => !a.Ukryty).OrderByDescending(a => a.DataDodania).Take(3).ToList();

                    var bestseller = _context.Products.Where(a => !a.Ukryty).OrderBy(a => Guid.NewGuid()).Take(3).ToList();
                    //Category category = new Category { Name = "Kategoria1", Description = "Opis kategoria", NazwaPlikuIkony = "ikona.png" };
                    //_context.Add(category);
                    //_context.SaveChanges();

                    //zainicjuj view model
                    var vm = new HomeViewModel() { Kategorie = kategorie, Nowosci = nowosci, Bestsellery = bestseller };

                    return View(vm); //zapewnia renderowania widoków 
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [Route("polityka-prywatnosci")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Redirect()
        {
            return RedirectToAction("Privacy");
        }

        [Route("testowy-route/{name}")]
        public IActionResult Product(string name)
        {
            return View();
        }

        public void PrawidlowaStrona()
        {

            //DodajProdukty();

            //zawsze trzeba pobrac dane i wrzucic do widoku
            var kategorie = _context.Category.ToList();

            //pobieramu produkty
            var nowosci = _context.Products.Where(a => !a.Ukryty).OrderByDescending(a => a.DataDodania).Take(3).ToList();

            var bestseller = _context.Products.Where(a => !a.Ukryty && a.Bestseller).OrderBy(a => Guid.NewGuid()).Take(3).ToList();
            //Category category = new Category { Name = "Kategoria1", Description = "Opis kategoria", NazwaPlikuIkony = "ikona.png" };
            //_context.Add(category);
            //_context.SaveChanges();

            //zainicjuj view model
            var vm = new HomeViewModel() { Kategorie = kategorie, Nowosci = nowosci, Bestsellery = bestseller };

           // return View(vm); //zapewnia renderowania widoków 
        }


        public void DodajProdukty()
        {

            //dodaj produkt do bazy
            //Product product1 = new Product {CategoryId=25, Name = "Produkt1", Description="Opis", NazwaPlikuObrazka="produkt1.jpg", Ukryty=true, DataDodania=DateTime.Now, CenaProduktu = 20, Bestseller = true };
            //_context.Product.Add(product1);


            //Dodanie przykladowyh produktow do bazy
            var prodykty = new List<Product>
            {
                new Product() { CategoryId = 16 ,CenaProduktu=200, Name= "Product1", Bestseller=true ,
                    NazwaPlikuObrazka="produkt1.jpg", DataDodania=DateTime.Now, Ukryty=false, Description="Opis produktu" },
                new Product() { CategoryId = 16 ,CenaProduktu=10, Name= "Product2", Bestseller=true ,
                    NazwaPlikuObrazka="produkt1.jpg", DataDodania=DateTime.Now, Ukryty=false, Description="Opis produktu" },
                new Product() { CategoryId = 16 ,CenaProduktu=150, Name= "Product3", Bestseller=true ,
                    NazwaPlikuObrazka="produkt1.jpg", DataDodania=DateTime.Now, Ukryty=false, Description="Opis produktu" },
                new Product() { CategoryId = 16 ,CenaProduktu=70, Name= "Product4", Bestseller=true ,
                    NazwaPlikuObrazka="produkt1.jpg", DataDodania=DateTime.Now, Ukryty=false, Description="Opis produktu" },
            };
            prodykty.ForEach(k => _context.Add(k));
            _context.SaveChanges();


        }
    }
}