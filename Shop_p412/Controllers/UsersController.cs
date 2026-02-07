using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Shop_p412.Controllers
{
    public class UsersController : Controller
    {
        public UserManager<IdentityUser> _userManager {  get; set; }
        public RoleManager<IdentityRole> _roleManager { get; set; }
        public SignInManager<IdentityUser> _signInManager { get; set; }
        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        //Створити GET методи для тестування VIEWS
        //http://localhost:[port]/users/register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Email,PasswordHash")] IdentityUser newUser)
        {
            if(ModelState.IsValid)
            {
                newUser.UserName = newUser.Email;
                newUser.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(newUser, newUser.PasswordHash);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return BadRequest(Json(result.Errors));
                }
            }
            return BadRequest("Error validation user... [02]");
        }
        [HttpGet]
        public IActionResult Login() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,PasswordHash")] IdentityUser user)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                        user.Email,
                        user.PasswordHash,
                        isPersistent: false,
                        lockoutOnFailure: false
                    );
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return BadRequest("Error email or password ... [01]");
                }
            }
            return BadRequest("Error email or password ... [02]");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
