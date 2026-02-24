using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_app_p32.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class APICartController : Controller
{
    private readonly ShopContext _context;
    private readonly UserManager<ShopUser> _userManager;

    public APICartController(
        ShopContext context,
        UserManager<ShopUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId)
    {
        var user = await _userManager.GetUserAsync(User);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (cart == null)
        {
            cart = new Cart { UserId = user.Id };
            _context.Carts.Add(cart);
        }

        var existingItem = cart.Items
            .FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
            existingItem.Quantity++;
        else
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Quantity = 1
            });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}