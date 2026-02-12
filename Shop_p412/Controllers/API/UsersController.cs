using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Shop_p412.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {

        public UserManager<IdentityUser> _userManager { get; set; }
        public RoleManager<IdentityRole> _roleManager { get; set; }
        public SignInManager<IdentityUser> _signInManager { get; set; }
        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] IdentityUser newUser)
        {
            if (ModelState.IsValid)
            {
                newUser.UserName = newUser.Email;
                newUser.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(newUser, newUser.PasswordHash);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(Json(result.Errors));
                }
            }
            return BadRequest(new { error = "Error validation user... [02]" });
        }
        //[HttpGet]
        //public IActionResult Login() => View();
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login([Bind("Email,PasswordHash")] IdentityUser user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(
        //                user.Email,
        //                user.PasswordHash,
        //                isPersistent: false,
        //                lockoutOnFailure: false
        //            );
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            return BadRequest("Error email or password ... [01]");
        //        }
        //    }
        //    return BadRequest("Error email or password ... [02]");
        //}
        //[HttpGet]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Index", "Home");
        //}
        //[HttpGet]
        //public IActionResult CreateRole() => View();
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateRole([Bind("Name")] IdentityRole newRole)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _roleManager.CreateAsync(newRole);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return BadRequest("Error name ... [01]");
        //}
        //[HttpGet]
        //public IActionResult AssignRole() => View();
        //[HttpPost]
        //public async Task<IActionResult> AssignRole(string roleId, string userId)
        //{
        //    if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(userId))
        //    {
        //        return BadRequest("userId ot roleId error ...");
        //    }
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return BadRequest("userId is error ...");
        //    }
        //    var role = await _roleManager.FindByIdAsync(roleId);
        //    if (role == null)
        //    {
        //        return BadRequest("roleId is error ...");
        //    }
        //    var result = await _userManager.AddToRoleAsync(user, role.Name);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return BadRequest(Json(result));
        //}
    }
}
