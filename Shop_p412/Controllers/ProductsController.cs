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
        public IActionResult CreateProduct() => View();
        [HttpPost]
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
    }
}
