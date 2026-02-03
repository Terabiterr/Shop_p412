using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop_p412.Services;

namespace Shop_p412
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ShopContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<UsersContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IServiceProduct, ServiceProduct>();
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;

                // Set custom password requirements
                options.Password.RequireDigit = false; // No digit required
                options.Password.RequireNonAlphanumeric = false; // Â³äì³íí³ characters required
                options.Password.RequiredLength = 4; // Minimum length of 4 characters
                options.Password.RequireUppercase = false; // No uppercase letter required
                options.Password.RequireLowercase = false; // No lowercase letter required
                options.Password.RequiredUniqueChars = 0; // No unique characters required

            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UsersContext>();
            builder.Services.AddControllersWithViews();
            var app = builder.Build();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );

            app.Run();
        }
    }
}
