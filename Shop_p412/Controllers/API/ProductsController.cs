using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop_p412.Services;

namespace Shop_p412.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IServiceProduct _serviceProduct;
        public ProductsController(IServiceProduct serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _serviceProduct.GetAllAsync();
            if(products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>  GetById(int id)
        {
            var product = await _serviceProduct.GetByIdAsync(id);
            if(product == null)
            {
                return NotFound(); 
            }
            return Ok(product);
        }
        //Створити реалізацію методів UpdateProduct, DeleteProduct
        //Протестувати методи в Postman
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            var product_update = await _serviceProduct.UpdateAsync(id, product);
            if(product_update == null)
            {
                return NotFound(); 
            }
            return Ok(product_update);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _serviceProduct.DeleteAsync(id);
            if( product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
