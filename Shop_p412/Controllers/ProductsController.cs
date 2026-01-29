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

        [HttpGet]
        public async Task<IActionResult> ReadProducts()
        {
            //Get product from database
            return View(/*products*/);
        }
        [HttpGet]
        public IActionResult CreateProduct() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([Bind("Name,Price,Description")]Product  product)
        {
            if(ModelState.IsValid)
            {
                //Add product to database
            }
            else
            {
                //return error
            }
            return RedirectToAction("ReadProducts", "Products");
        }
    }
}
