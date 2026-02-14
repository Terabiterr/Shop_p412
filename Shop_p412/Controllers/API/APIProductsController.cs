using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop_p412.Services;

namespace Shop_p412.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]  // Авторизация с использованием схемы JWT Bearer
    /*
     Додати реалізацію в APIUsersController методи CreateRole та AssignRole
 
    Реєстрація та авторизація користувача через API та протестувати APIProductsController
 
    Підготувати скриншоти тестування
 
     */
    public class APIProductsController : Controller
    {
        private readonly IServiceProduct _serviceProduct;
        public APIProductsController(IServiceProduct serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }
        [HttpPost]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            var result = await _serviceProduct.CreateAsync(product);
            if(result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
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
        [Authorize(Roles = "admin,moderator")]
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
        [Authorize(Roles = "admin,moderator")]
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
