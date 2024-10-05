using backend.Context;
using backend.Dtos;
using backend.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        public ProductsController(ApplicationDBContext dBContext) {
            _dbContext = dBContext;
        }

        //Create

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateUpdateProductDto productDto)
        {
            var newProduct = new ProductEntity
            {
                Brand = productDto.Brand,
                Title = productDto.Title,
            };
            await _dbContext.Products.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();
            return Ok("Product Saved Successfully");
        }

        //Read
        [HttpGet]
        public async Task<ActionResult<List<ProductEntity>>> GetAllProducts()
        {
            var products = await _dbContext.Products.OrderByDescending(x=>x.UpdatedAt).ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductEntity>> GetProductById([FromRoute] long id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if(product is null)
            {
                return NotFound("Product not Found");
            }
            return Ok(product);
        }

        //Update
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] long id, [FromBody] CreateUpdateProductDto updateProductDto)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if(product is null)
            {
                return NotFound("Product not Found.");
            }

            product.Brand = updateProductDto.Brand;
            product.Title = updateProductDto.Title;
            product.UpdatedAt = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return Ok("Product updated Successfully.");
        }

        //Delete
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] long id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
            {
                return NotFound("Product not Found.");
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return Ok("Product Deleted Successfully.");
        }
    }
}
