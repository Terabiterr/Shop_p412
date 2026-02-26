using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Error length [max = 50, min = 3] ...")]
    public string? Name { get; set; }
    [Required]
    [Precision(10, 2)]
    public decimal Price { get; set; }
    [Required]
    [StringLength(1024)]
    public string? Description { get; set; }
    [Required]
    [Range(0, Int32.MaxValue)]
    public int Quantity { get; set; }
    [Required]
    public int CategoryId { get; set; }
    //[JsonIgnore]
    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }
    public ICollection<OrderItem>? OrderItems { get; set; }
    public ICollection<CartItem>? CartItems { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    public ICollection<ProductImage>? ProductImages { get; set; }

}
