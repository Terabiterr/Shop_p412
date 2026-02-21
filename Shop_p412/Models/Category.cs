using Shop_app_p32.Models;
using System.ComponentModel.DataAnnotations;

public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}