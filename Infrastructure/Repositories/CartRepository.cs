using Application.DTOs;
using Application.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DapperContext _context;
        public CartRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            var sql = "SELECT * FROM cart WHERE user_id = @userId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Cart>(sql, new { userId });
        }

        public async Task<int> CreateCartAsync(Guid userId)
        {
            var sql = "INSERT INTO cart (user_id) VALUES (@userId); SELECT LAST_INSERT_ID();";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new { userId });
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int plantId)
        {
            var sql = "SELECT * FROM cart_items WHERE cart_id = @cartId AND plant_id = @plantId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<CartItem>(sql, new { cartId, plantId });
        }

        public async Task<bool> AddCartItemAsync(int cartId, int plantId, int quantity)
        {
            var sql = @"INSERT INTO cart_items (cart_id, plant_id, quantity)
                        VALUES (@cartId, @plantId, @quantity)";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { cartId, plantId, quantity });
            return result > 0;
        }

        public async Task<bool> IncreaseQuantityAsync(int cartItemId)
        {
            var sql = "UPDATE cart_items SET quantity = quantity + 1 WHERE id = @cartItemId";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(sql, new { cartItemId });
            return result > 0;
        }

        public async Task<CartItem?> GetCartItemAsync(Guid userId, int plantId)
        {
            var sql = @"SELECT ci.* FROM cart_items ci
                    JOIN cart c ON ci.cart_id = c.id
                    WHERE c.user_id = @userId AND ci.plant_id = @plantId";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<CartItem>(sql, new { userId, plantId });
        }

        public async Task<int> GetPlantStockAsync(int plantId)
        {
            var sql = "SELECT stock FROM plants WHERE id = @plantId";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, new { plantId });
        }

        public async Task<bool> RemoveCartItemAsync(Guid userId, int plantId)
        {
            var sql = @"DELETE FROM cart_items 
                    WHERE cart_id = (SELECT id FROM cart WHERE user_id = @userId)
                    AND plant_id = @plantId";
            using var connection = _context.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new { userId, plantId });
            return affected > 0;
        }

        public async Task<bool> IncreaseQuantityAsync(Guid userId, int plantId)
        {
            var sql = @"UPDATE cart_items
                    SET quantity = quantity + 1
                    WHERE cart_id = (SELECT id FROM cart WHERE user_id = @userId)
                    AND plant_id = @plantId";
            using var connection = _context.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new { userId, plantId });
            return affected > 0;
        }

        public async Task<bool> DecreaseQuantityAsync(Guid userId, int plantId)
        {
            var sql = @"UPDATE cart_items
                    SET quantity = quantity - 1
                    WHERE cart_id = (SELECT id FROM cart WHERE user_id = @userId)
                    AND plant_id = @plantId";
            using var connection = _context.CreateConnection();
            var affected = await connection.ExecuteAsync(sql, new { userId, plantId });
            return affected > 0;
        }
        public async Task<bool> IncreaseDurationAsync(Guid userId, int plantId)
        {
            var sql = @"
        UPDATE cart_items
        SET duration_months = duration_months + 1
        WHERE plant_id = @PlantId AND cart_id = (SELECT id FROM cart WHERE user_id = @UserId)";
            using var connection = _context.CreateConnection();

            var result = await connection.ExecuteAsync(sql, new { PlantId = plantId, UserId = userId });
            return result > 0;
        }

        public async Task<bool> DecreaseDurationAsync(Guid userId, int plantId)
        {
            var sql = @"
        UPDATE cart_items
        SET duration_months = duration_months - 1
        WHERE plant_id = @PlantId AND cart_id = (SELECT id FROM cart WHERE user_id = @UserId)
          AND duration_months > 1";
            using var connection = _context.CreateConnection();

            var result = await connection.ExecuteAsync(sql, new { PlantId = plantId, UserId = userId });
            return result > 0;
        }

        public async Task<List<CartItemDTO>> GetCartItemsByUserAsync(Guid userId)
        {
            var sql = @"
        SELECT 
            ci.id AS CartItemId,
            p.id AS PlantId,
            p.name AS PlantName,
            p.image_url AS PlantImage, -- assuming this exists
            p.price_per_month AS UnitPrice,
            ci.quantity,
            ci.duration_months
        FROM cart_items ci
        JOIN cart c ON ci.cart_id = c.id
        JOIN plants p ON ci.plant_id = p.id
        WHERE c.user_id = @UserId";
            using var connection = _context.CreateConnection();


            var result = await connection.QueryAsync<CartItemDTO>(sql, new { UserId = userId });
            return result.ToList();
        }



    }
}
