using Shop_app_p32.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProductImage
{
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string ImageUrl { get; set; } = null!;

    [Required]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
}