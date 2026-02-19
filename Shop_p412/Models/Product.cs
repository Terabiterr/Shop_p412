using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    //Image product
    [NotMapped]
    public IFormFile? ImageData { get; set; }
    //Format image
    public string? ImageType { get; set; }
    public byte[]? ImageFile { get; set; }
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Price: {Price}, Description: {Description}";
    }

}
