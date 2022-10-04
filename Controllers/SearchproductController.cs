using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using partner_aluro.DAL;

namespace partner_aluro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchproductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public SearchproductController(ApplicationDbContext db)
        {
            _db = db;   
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                //string term = HttpContext.Request.Query["term"];
                string term = HttpContext.Request.Query["term"];
                var postTitle = _db.Products.Where(p => p.Name.Contains(term))
                                .Select(p => p.Name).ToList();
                return Ok(postTitle);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
