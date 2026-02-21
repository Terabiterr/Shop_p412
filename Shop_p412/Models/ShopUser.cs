using Microsoft.AspNetCore.Identity;

namespace Shop_app_p32.Models
{
    public class ShopUser : IdentityUser
    {
        //One user to many orders
        public ICollection<Order>? Orders { get; set; }
        //One user to one Cart
        public Cart? Cart { get; set; }
        //One User to many Reviews
        public ICollection<Review>? Reviews { get; set; }
    }
}