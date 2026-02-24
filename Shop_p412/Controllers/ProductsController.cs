using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProductsController : Controller
{
    private readonly ShopContext _context;

    public ProductsController(ShopContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> ReadProducts()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> DetailsProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Include(p => p.Reviews)
            .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        return View(product);
    }

    public IActionResult CreateProduct()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        _context.Add(product);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}