using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class APICategoriesController : Controller
{
    private readonly ShopContext _context;

    public APICategoriesController(ShopContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Categories.ToListAsync());
    }


    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid)
            return View(category);

        _context.Add(category);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}