using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shop_app_p32.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop_p412.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIUsersController : Controller
    {

        public UserManager<ShopUser> _userManager { get; set; }
        public RoleManager<IdentityRole> _roleManager { get; set; }
        public SignInManager<ShopUser> _signInManager { get; set; }
        public IConfiguration _configuration { get; set; }
        public APIUsersController(UserManager<ShopUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ShopUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ShopUser newUser)
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
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ShopUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                        user.Email,
                        user.PasswordHash,
                        isPersistent: false,
                        lockoutOnFailure: false
                    );
         
                if (result.Succeeded)
                {
                    var jwt_token = GenerateJwtToken(user);
                    return Ok(new { token = jwt_token });
                }
                else
                {
                    return BadRequest("Error email or password ... [01]");
                }
            }
            return BadRequest("Error email or password ... [02]");
        }
        private async Task<string> GenerateJwtToken(ShopUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user); // 🟢 Отримуємо ролі

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // 🟢 Додаємо ролі в claims
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
