using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;

namespace partner_aluro.Controllers
{
    public class ProductImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductImages.ToListAsync());
        }

        //GET: productImage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound(); 
            }

            var productImage = await _context.ProductImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if(productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        //GET: ProductImages/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST ProductImages/.Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageURL")] ProductImages productImages)
        {
            if(ModelState.IsValid)
            {
                _context.Add(productImages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productImages);
        }
        // GET: ProductImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductImages == null)
            {
                return NotFound();
            }

            var productImages = await _context.ProductImages.FindAsync(id);
            if (productImages == null)
            {
                return NotFound();
            }
            return View(productImages);
        }

        // POST: ProductImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageURL")] ProductImages productImages)
        {
            if (id != productImages.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productImages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductImagesExists(productImages.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productImages);
        }

        // GET: ProductImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductImages == null)
            {
                return NotFound();
            }

            var productImages = await _context.ProductImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImages == null)
            {
                return NotFound();
            }

            return View(productImages);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductImages == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProductImages'  is null.");
            }
            var productImages = await _context.ProductImages.FindAsync(id);
            if (productImages != null)
            {
                _context.ProductImages.Remove(productImages);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductImagesExists(int id)
        {
            return _context.ProductImages.Any(e => e.Id == id);
        }
    }
}
