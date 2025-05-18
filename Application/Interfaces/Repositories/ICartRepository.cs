using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{

        public interface ICartRepository
        {
            Task<Cart?> GetCartByUserIdAsync(Guid userId);
            Task<int> CreateCartAsync(Guid userId);
            Task<CartItem?> GetCartItemAsync(int cartId, int plantId);
            Task<bool> AddCartItemAsync(int cartId, int plantId, int quantity);
            Task<bool> IncreaseQuantityAsync(int cartItemId);
            Task<int> GetPlantStockAsync(int plantId);
            Task<bool> RemoveCartItemAsync(Guid userId, int plantId);
            Task<bool> IncreaseQuantityAsync(Guid userId, int plantId);
            Task<bool> DecreaseQuantityAsync(Guid userId, int plantId);
            Task<CartItem?> GetCartItemAsync(Guid userId, int plantId);
            Task<bool> IncreaseDurationAsync(Guid userId, int plantId);
            Task<bool> DecreaseDurationAsync(Guid userId, int plantId);
            Task<List<CartItemDTO>> GetCartItemsByUserAsync(Guid userId);

    }
}

