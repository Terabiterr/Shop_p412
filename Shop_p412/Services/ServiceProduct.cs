using Microsoft.EntityFrameworkCore;

namespace Shop_p412.Services
{
    public interface IServiceProduct
    {
        public Task<Product> CreateAsync(Product product);
        public Task<IEnumerable<Product>> GetAll();
        public Task<Product> GetByIdAsync(int id);
        public Task<Product> UpdateAsync(int id, Product product);
        public void DeleteAsync(int id);

    }
    public class ServiceProduct : IServiceProduct
    {
        private readonly ShopContext _db;
        public ServiceProduct(ShopContext db)
        {
            _db = db;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }
        public async void DeleteAsync(int id)
        {
            var product = await _db.Products.FirstAsync(p => p.Id == id);
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAll() => await _db.Products.ToListAsync();

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }
            return product;
        }

        public async Task<Product> UpdateAsync(int id, Product product)
        {
            var product_for_update = await GetByIdAsync(id);
            product_for_update.Name = product.Name;
            product_for_update.Description = product.Description;
            product_for_update.Price = product.Price;
            await _db.SaveChangesAsync();
            return product_for_update;
        }
    }
}
