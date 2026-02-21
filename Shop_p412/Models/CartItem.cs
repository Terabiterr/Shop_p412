using Shop_app_p32.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CartItem
{
    public int Id { get; set; }

    [Required]
    public int CartId { get; set; }

    [ForeignKey(nameof(CartId))]
    public Cart Cart { get; set; } = null!;

    [Required]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;

    [Range(1, 1000)]
    public int Quantity { get; set; }
}