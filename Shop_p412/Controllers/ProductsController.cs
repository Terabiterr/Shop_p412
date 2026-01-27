using Microsoft.AspNetCore.Mvc;

namespace Shop_p412.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
