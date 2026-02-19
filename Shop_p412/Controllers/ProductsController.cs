using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_p412.Services;

namespace Shop_p412.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IServiceProduct _serviceProduct;
        public ProductsController(IServiceProduct serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }

        //https://localhost:[port]/products/readproducts
        //HTTP METHOD: GET
        [HttpGet]
        public async Task<IActionResult> ReadProducts()
        {
            var products = await _serviceProduct.GetAllAsync();
            return View(products);
        }
        [HttpGet]
        [Authorize(Roles = "admin,moderator")]
        public IActionResult CreateProduct() => View();
        [HttpPost]
        [Authorize(Roles = "admin,moderator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([Bind("Name,Price,Description")]Product  product)
        {
            if(ModelState.IsValid)
            {
                _ = await _serviceProduct.CreateAsync(product);
                return RedirectToAction("ReadProducts");
            }
            return BadRequest("Error model product ...");
        }
        [HttpGet]
        [Authorize(Roles = "admin,moderator")]
        public IActionResult UpdateProduct() => View();
        [HttpPost]
        [Authorize(Roles = "admin,moderator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(int id, [Bind("Name,Price,Description")] Product product)
        {
            if(ModelState.IsValid)
            {
                _ = await _serviceProduct.UpdateAsync(id, product);
                return RedirectToAction("ReadProducts");
            }
            return BadRequest("Error model product ...");
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "admin,moderator")]
        public IActionResult GetDeleteProduct(int id) => View("DeleteProduct", id);
        [HttpPost]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _serviceProduct.DeleteAsync(id);
            return RedirectToAction("ReadProducts");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DetailsProduct(int id)
        {
            var product = await _serviceProduct.GetByIdAsync(id);
            return View(product);
        }
    }
}
