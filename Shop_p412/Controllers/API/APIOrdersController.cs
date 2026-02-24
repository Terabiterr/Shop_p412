using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_app_p32.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class APIOrdersController : Controller
{
    private readonly ShopContext _context;
    private readonly UserManager<ShopUser> _userManager;

    public APIOrdersController(
        ShopContext context,
        UserManager<ShopUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        var user = await _userManager.GetUserAsync(User);

        var orders = await _context.Orders
            .Where(o => o.UserId == user.Id)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();

        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        var user = await _userManager.GetUserAsync(User);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (cart == null || !cart.Items.Any())
            return RedirectToAction("Index", "Cart");

        var order = new Order
        {
            UserId = user.Id,
            Items = cart.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Product.Price
            }).ToList(),
            TotalPrice = cart.Items.Sum(i => i.Quantity * i.Product.Price)
        };

        _context.Orders.Add(order);

        _context.CartItems.RemoveRange(cart.Items);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(MyOrders));
    }
}