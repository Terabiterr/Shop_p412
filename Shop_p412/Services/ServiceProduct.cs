using Microsoft.EntityFrameworkCore;
/*
 INSERT INTO ShopUser
(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
 PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
 TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES
('11111111-1111-1111-1111-111111111111', 'john.smith', 'JOHN.SMITH',
 'john.smith@gmail.com', 'JOHN.SMITH@GMAIL.COM', 1,
 'AQAAAAIAAYagAAAAEFAKEHASH1==', NEWID(), NEWID(),
 '+380501112233', 1, 0, 0, 0),

('22222222-2222-2222-2222-222222222222', 'anna.brown', 'ANNA.BROWN',
 'anna.brown@gmail.com', 'ANNA.BROWN@GMAIL.COM', 1,
 'AQAAAAIAAYagAAAAEFAKEHASH2==', NEWID(), NEWID(),
 '+380671234567', 1, 0, 0, 0),

('33333333-3333-3333-3333-333333333333', 'michael.jones', 'MICHAEL.JONES',
 'michael.jones@gmail.com', 'MICHAEL.JONES@GMAIL.COM', 1,
 'AQAAAAIAAYagAAAAEFAKEHASH3==', NEWID(), NEWID(),
 '+380931112233', 1, 0, 0, 0);
--🔹 2. Категорії (якщо є таблиця Category)
INSERT INTO Categories (Name)
VALUES 
('Smartphones'),
('Laptops'),
('Accessories');
--🔹 3. Продукти
INSERT INTO Products (Name, Price, Description, Quantity, CategoryId)
VALUES
('iPhone 15 Pro', 1299.99, 'Apple flagship smartphone with A17 chip', 15, 1),
('Samsung Galaxy S24', 1099.50, 'Latest Samsung smartphone with AMOLED display', 20, 1),
('MacBook Air M3', 1599.00, 'Lightweight laptop with Apple M3 processor', 8, 2),
('Dell XPS 13', 1399.99, 'Premium ultrabook with Intel i7 processor', 10, 2),
('Sony WH-1000XM5', 399.99, 'Noise cancelling wireless headphones', 30, 3);
--🔹 4. Кошики (One User → One Cart)
INSERT INTO Carts (UserId)
VALUES
('11111111-1111-1111-1111-111111111111'),
('22222222-2222-2222-2222-222222222222'),
('33333333-3333-3333-3333-333333333333');
--🔹 5. Замовлення
INSERT INTO Orders (CreatedAt, TotalPrice, UserId)
VALUES
(GETUTCDATE(), 1699.98, '11111111-1111-1111-1111-111111111111'),
(GETUTCDATE(), 399.99, '22222222-2222-2222-2222-222222222222');
--🔹 6. OrderItems
-- Order 1 (John купив iPhone + Headphones)
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
VALUES
(1, 1, 1, 1299.99),
(1, 5, 1, 399.99);

-- Order 2 (Anna купила Headphones)
INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
VALUES
(2, 5, 1, 399.99);
--🔹 7. CartItems (якщо є таблиця CartItems)
INSERT INTO CartItems (CartId, ProductId, Quantity)
VALUES
(4, 2, 1),
(5, 3, 1),
(3, 2, 2);
 */
namespace Shop_p412.Services
{
    public interface IServiceProduct
    {
        public Task<Product> CreateAsync(Product product);
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<Product> GetByIdAsync(int id);
        public Task<Product> UpdateAsync(int id, Product product);
        public Task<Product> DeleteAsync(int id);

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
            if(product == null)
            {
                return product;
            }
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }
        public async Task<Product> DeleteAsync(int id)
        {
            var product = await _db.Products.FirstAsync(p => p.Id == id);
            if(product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _db.Products.ToListAsync();

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
