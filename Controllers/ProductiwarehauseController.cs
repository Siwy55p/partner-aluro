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

namespace partner_aluro.Controllers
{
    [Authorize]
    public class ProductiwarehauseController : Controller
    {
        private readonly IUnitOfWorkProduct _unitOfWorkProduct;
        private readonly IProductService _warehouseService;
        public ProductiwarehauseController(IProductService warehauseService, IUnitOfWorkProduct unitOfWorkProduct)
        {
            _warehouseService = warehauseService;
            _unitOfWorkProduct = unitOfWorkProduct;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var produkt = _warehouseService.GetProductId(id);

            var ListaKategorii = _warehouseService.GetCategory();
            var categoryList = _warehouseService.GetCategory().ToList();

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

            var ListaKategorii = _warehouseService.GetCategory();
            var categoryList = _warehouseService.GetCategory().ToList();

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

            produkt.Ukryty = false;
            produkt.Bestseller = true;

            var ListaKategorii = _warehouseService.GetCategory();

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
            if (!ModelState.IsValid)
            {

                return View(addProductModel);
            }

            if (produkt != null)
            {
                var id = _warehouseService.AddProduct(produkt); //wazne aby przypisac
            }

            return RedirectToAction("List");

        }

        [HttpGet]
        public IActionResult List()
        {
            var produkty = _warehouseService.GetProductList();

            return View(produkty);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _warehouseService.GetProductId(id);
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _warehouseService.DeleteProductId(id);
            return RedirectToAction("List");
        }
    }
}
