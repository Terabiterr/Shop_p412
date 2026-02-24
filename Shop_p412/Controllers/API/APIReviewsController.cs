using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_app_p32.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class APIReviewsController : Controller
{
    private readonly ShopContext _context;
    private readonly UserManager<ShopUser> _userManager;

    public APIReviewsController(
        ShopContext context,
        UserManager<ShopUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create(int productId, int rating, string comment)
    {
        var user = await _userManager.GetUserAsync(User);

        var review = new Review
        {
            ProductId = productId,
            Rating = rating,
            Comment = comment,
            UserId = user.Id
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Products", new { id = productId });
    }
}