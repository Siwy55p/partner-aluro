using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.DAL;

namespace partner_aluro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostApi : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public PostApi(ApplicationDbContext db)
        {
            _db = db;       
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Serach()
        {
            string term = "";
            var posTitle = _db.Products.Where(p => p.Name.Contains(term)).Select(p => p.Name).ToList();

            return Ok(posTitle);


        }
    }
}
