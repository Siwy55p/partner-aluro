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

namespace partner_aluro.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IUnitOfWorkProduct _unitOfWorkProduct;
        private readonly IProductService _ProductService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext applicationDbContext, IProductService productService, IUnitOfWorkProduct unitOfWorkProduct, IWebHostEnvironment webHostEnvironment)
        {
            _ProductService = productService;
            _unitOfWorkProduct = unitOfWorkProduct;
            _webHostEnvironment = webHostEnvironment;
            _context = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var produkt = _ProductService.GetProductId(id);

            var ListaKategorii = _ProductService.GetCategory();
            var categoryList = _ProductService.GetCategory().ToList();

            var roleItems = ListaKategorii.Select(cat =>
                new SelectListItem(
                    cat.Name,
                    cat.Description
                    )).ToList();

            var vm = new AddProductFormModel
            {
                Product = produkt,
                Categories = roleItems,
            };


            return View(vm);
        }

        [HttpPost]
        public IActionResult EditOnPost(AddProductFormModel dataProduct)
        {
            var produkt = _unitOfWorkProduct.Product.GetProductId(dataProduct.Product.ProductId);


            produkt.Name = dataProduct.Product.Name;
            produkt.Symbol = dataProduct.Product.Symbol;
            produkt.Description = dataProduct.Product.Description;
            produkt.CenaProduktu = dataProduct.Product.CenaProduktu;
            produkt.CenaProduktuDetal = dataProduct.Product.CenaProduktuDetal;
            produkt.WagaProduktu = dataProduct.Product.WagaProduktu;
            produkt.SzerokoscProduktu = dataProduct.Product.SzerokoscProduktu;
            produkt.WysokoscProduktu = dataProduct.Product.WysokoscProduktu;
            produkt.GlebokoscProduktu = dataProduct.Product.WagaProduktu;
            produkt.Bestseller = dataProduct.Product.Bestseller;
            produkt.Ukryty = dataProduct.Product.Ukryty;

            _unitOfWorkProduct.Product.UpdateProduct(produkt);


            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Add()
        {

            var ListaKategorii = _ProductService.GetCategory();
            var categoryList = _ProductService.GetCategory().ToList();

            var roleItems = ListaKategorii.Select(cat =>
                new SelectListItem(
                    cat.Name,
                    cat.Description
                    )).ToList();

            var vm = new AddProductFormModel
            {
                Categories = roleItems
            };


            return View(vm);
        }

        [HttpPost]
        public IActionResult Add(AddProductFormModel addProductModel)
        {
            Product produkt = addProductModel.Product;
            produkt.DataDodania = DateTime.Now;

            string uniqueFileName = UploadFile(produkt);
            produkt.ImageUrl = uniqueFileName;
            

            produkt.Ukryty = false;
            produkt.Bestseller = true;

            var ListaKategorii = _ProductService.GetCategory();

            string text;
            int idCatSelected =0;

            foreach (var categoris in addProductModel.Categories)
            {
                if (categoris.Selected == true)
                {
                    text = categoris.Text;
                    foreach (var listAllCat in ListaKategorii)
                    {
                        if (listAllCat.Name == text)
                        {
                            idCatSelected = listAllCat.CategoryId;
                        }
                    }

                }
            }
            if (idCatSelected != 0)
            {
                produkt.CategoryId = idCatSelected;
            }

            addProductModel.Product = produkt;
            ModelState.Remove("Product.ProductId");
            ModelState.Remove("Product.ImageUrl");
            if (!ModelState.IsValid)
            {

                return View(addProductModel);
            }

            if (produkt != null)
            {
                var id = _ProductService.AddProduct(produkt); //wazne aby przypisac
            }

            return RedirectToAction("List");

        }

        [HttpGet]
        public IActionResult List()
        {
            var produkty = _ProductService.GetProductList();

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
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + product.FrontImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.FrontImage.CopyTo(fileStream);
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
