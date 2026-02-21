using Shop_app_p32.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Cart
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public ShopUser User { get; set; } = null!;

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}