using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DallalCore.Data;
using DallalCore.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DallalCore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DallalCoreContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(DallalCoreContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Product.Include(x=>x.address).Include(x=>x.user).Include(x=>x.category).Where(x=>!x.isBlocked).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductForAdmin()
        {
            return await _context.Product.Include(x => x.address).Include(x => x.user).Include(x => x.category).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByUserId(int id)
        {
            return await _context.Product.Include(x => x.address).Include(x => x.user).Include(x => x.category).Where(x=>x.userId==id).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // GET: api/Products/5
        [HttpGet("{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            int categoryId = _context.Category.Where(x => x.category == category).Select(x=>x.categoryID).FirstOrDefault();
            var product = await _context.Product.Include(x => x.address).Include(x => x.user).Include(x => x.category).Where(x=>x.categoryId==categoryId && !x.isBlocked).ToListAsync();

            return product;
        }

        // GET: api/Products/5
        [HttpGet("{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryForAdmin(string category)
        {
            int categoryId = _context.Category.Where(x => x.category == category).Select(x => x.categoryID).FirstOrDefault();
            var product = await _context.Product.Include(x => x.address).Include(x => x.user).Include(x => x.category).Where(x => x.categoryId == categoryId).ToListAsync();

            return product;
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByTitle(string title)
        {
            var product = await _context.Product.Include(x=>x.address).Include(x=>x.category).Include(x=>x.user).Where(x => x.title.Contains(title)).ToListAsync();

            return product;
        }



        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutProduct([FromForm] Product product, IFormFile Picture)
        {
          
            string path = _environment.WebRootPath + @"/images/";
            FileStream fileStream;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fileStream = System.IO.File.Create(path + "product_image" + product.productId + "." + Picture.ContentType.Split("/")[1]);
            Picture.CopyTo(fileStream);
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
            product.pics_path = @"/images/" + "product_image" + product.productId + "." + Picture.ContentType.Split("/")[1];

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
          
                
                    throw;
                
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm]Product product,IFormFile Picture)
        {
            if (Picture != null)
            {
                try
                {
                    product.dateTime = DateTime.Now;
                    product.isBlocked = false;
                    _context.Product.Add(product);
                    await _context.SaveChangesAsync();
                    string path = _environment.WebRootPath + @"/images/";
                    FileStream fileStream;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    fileStream = System.IO.File.Create(path + "product_image" + product.productId+product.dateTime.ToString() + "." + Picture.ContentType.Split("/")[1]);
                    Picture.CopyTo(fileStream);
                    fileStream.Flush();
                    fileStream.Close();
                    fileStream.Dispose();
                    product.pics_path = @"/images/" + "product_image" + product.productId+product.dateTime.ToString() + "." + Picture.ContentType.Split("/")[1];
                    _context.Entry(product).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch
                {

                }
            }
            else
                return BadRequest();
            

            return CreatedAtAction("GetProduct", new { id = product.productId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> BlockOrUnBlockProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.isBlocked = !product.isBlocked;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.productId == id);
        }
    }
}
