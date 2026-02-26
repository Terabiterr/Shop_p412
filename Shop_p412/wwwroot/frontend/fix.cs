========================================================
================ BACKEND (ИСПРАВЛЕННЫЕ API) ============

APICategoriesController

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class APICategoriesController : ControllerBase
{
    private readonly ShopContext _context;

    public APICategoriesController(ShopContext context)
    {
        _context = context;
    }

    // GET api/APICategories
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Categories.ToListAsync();
        return Ok(categories);
    }

    // POST api/APICategories
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return Ok(category);
    }
}

========================================================

APIProductsController

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class APIProductsController : ControllerBase
{
    private readonly IServiceProduct _serviceProduct;

    public APIProductsController(IServiceProduct serviceProduct)
    {
        _serviceProduct = serviceProduct;
    }

    // GET api/APIProducts
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _serviceProduct.GetAllAsync();
        return Ok(products);
    }

    // GET api/APIProducts/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _serviceProduct.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // POST api/APIProducts
    [HttpPost]
    [Authorize(Roles = "admin,moderator")]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        var result = await _serviceProduct.CreateAsync(product);
        return Ok(result);
    }

    // PUT api/APIProducts/5
    [HttpPut("{id}")]
    [Authorize(Roles = "admin,moderator")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        var result = await _serviceProduct.UpdateAsync(id, product);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // DELETE api/APIProducts/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,moderator")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _serviceProduct.DeleteAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}

========================================================

APICartController

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class APICartController : ControllerBase
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

    // GET api/APICart
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var user = await _userManager.GetUserAsync(User);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .Where(c => c.UserId == user.Id)
            .Select(c => new
            {
                Items = c.Items.Select(i => new
                {
                    i.ProductId,
                    i.Quantity,
                    i.Product.Name,
                    i.Product.Price
                })
            })
            .FirstOrDefaultAsync();

        return Ok(cart);
    }

    // POST api/APICart/add
    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] int productId)
    {
        var user = await _userManager.GetUserAsync(User);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = user.Id,
                Items = new List<CartItem>()
            };
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

        return Ok();
    }

    // DELETE api/APICart/5
    [HttpDelete("{productId}")]
    public async Task<IActionResult> Remove(int productId)
    {
        var user = await _userManager.GetUserAsync(User);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            return NotFound();

        cart.Items.Remove(item);
        await _context.SaveChangesAsync();

        return Ok();
    }
}

========================================================

APIOrdersController

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class APIOrdersController : ControllerBase
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

    // GET api/APIOrders
    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        var user = await _userManager.GetUserAsync(User);

        var orders = await _context.Orders
            .Where(o => o.UserId == user.Id)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();

        return Ok(orders);
    }

    // POST api/APIOrders/checkout
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout()
    {
        var user = await _userManager.GetUserAsync(User);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (cart == null || !cart.Items.Any())
            return BadRequest("Cart is empty");

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

        return Ok(order);
    }
}