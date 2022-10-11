//using AspNetCore;
//using Microsoft.AspNetCore.Mvc;
//using partner_aluro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.ViewModels;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using static partner_aluro.Core.Constants;
using partner_aluro.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using partner_aluro.DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private readonly IUnitOfWorkProduct _unitOfWorkProduct;
        private readonly IProductService _ProductService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext applicationDbContext, IProductService productService, IUnitOfWorkProduct unitOfWorkProduct, IWebHostEnvironment webHostEnvironment)
        {
            _ProductService = productService;
            _unitOfWorkProduct = unitOfWorkProduct;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Category = GetCategories();
            var product = _ProductService.GetProductId(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult EditOnPost(Product product)
        {
            var produkt = _unitOfWorkProduct.Product.GetProductId(product.ProductId);


            produkt.Name = product.Name;
            produkt.Symbol = product.Symbol;
            produkt.Description = product.Description;
            produkt.CenaProduktu = product.CenaProduktu;
            produkt.CenaProduktuDetal = product.CenaProduktuDetal;
            produkt.WagaProduktu = product.WagaProduktu;
            produkt.SzerokoscProduktu = product.SzerokoscProduktu;
            produkt.WysokoscProduktu = product.WysokoscProduktu;
            produkt.GlebokoscProduktu = product.WagaProduktu;
            produkt.Bestseller = product.Bestseller;
            produkt.Ukryty = product.Ukryty;

            produkt.CategoryNavigation = _ProductService.GetCategoryId(product.CategoryId);

            _unitOfWorkProduct.Product.UpdateProduct(produkt);

            string uniqueFileName = UploadFile(product);
            product.ImageUrl = uniqueFileName;

            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Category = GetCategories();
            Product product = new Product();

            return View(product);
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            ViewBag.Category = GetCategories();

            product.DataDodania = DateTime.Now;

            product.Ukryty = false;
            product.Bestseller = true;
            product.ImageUrl = "";

            product.CategoryNavigation = _ProductService.GetCategoryId(product.CategoryId);



            string uniqueFileName = UploadFile(product);
            product.ImageUrl = uniqueFileName;


            ModelState.Remove("CategoryNavigation");

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var id = _ProductService.AddProduct(product);//wazne aby przypisac
            return RedirectToAction("List");

        }

        [HttpGet]
        public IActionResult List()
        {
            List<Product> produkty = _ProductService.GetProductList();

            return View(produkty);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _ProductService.GetProductId(id);
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _ProductService.DeleteProductId(id);
            return RedirectToAction("List");
        }


        private string UploadFile(Product product)
        {
            string uniqueFileName = null;

            if(product.FrontImage != null)
            {
                ModelState.Remove("CategoryNavigation");

                product.ImageUrl = "";

                if (ModelState.IsValid)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\produkty");

                    uniqueFileName = DateTime.Now.ToString("yymmssfff") + product.FrontImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        product.FrontImage.CopyTo(fileStream);
                    }
                }
            }

            return uniqueFileName;
        }


        private List<SelectListItem> GetCategories()
        {
            var lstCategories = new List<SelectListItem>();

            lstCategories = _ProductService.GetListCategory().Select(ct => new SelectListItem()
            {
                Value = ct.CategoryId.ToString(),
                Text = ct.Name
            }).ToList();

            var dmyItem = new SelectListItem()
            {
                Value = null,
                Text = "--- Wybierz Kategorie ---"
            };

            lstCategories.Insert(0, dmyItem);
            return lstCategories;
        }
    }
}