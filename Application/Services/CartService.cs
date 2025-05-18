using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseDTO<string>> AddToCartAsync(Guid userId, int plantId)
        {
            try
            {
                var cart = await _repository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    int newCartId = await _repository.CreateCartAsync(userId);
                    await _repository.AddCartItemAsync(newCartId, plantId, 1);
                    return new ResponseDTO<string>
                    {
                        StatusCode = 200,
                        Message = "Cart created and plant added successfully"
                    };
                }

                var cartItem = await _repository.GetCartItemAsync(cart.Id, plantId);
                if (cartItem != null)
                {
                    await _repository.IncreaseQuantityAsync(cartItem.Id);
                    return new ResponseDTO<string>
                    {
                        StatusCode = 200,
                        Message = "Plant already in cart. Quantity increased."
                    };
                }

                await _repository.AddCartItemAsync(cart.Id, plantId, 1);
                return new ResponseDTO<string>
                {
                    StatusCode = 200,
                    Message = "Plant added to cart."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>
                {
                    StatusCode = 500,
                    Error = ex.Message
                };
            }
        }
        public async Task<ResponseDTO<string>> RemoveFromCartAsync(Guid userId, int plantId)
        {
            var success = await _repository.RemoveCartItemAsync(userId, plantId);
            return new ResponseDTO<string>
            {
                StatusCode = success ? 200 : 404,
                Message = success ? "Item removed successfully" : "Item not found"
            };
        }

        public async Task<ResponseDTO<string>> IncreaseQuantityAsync(Guid userId, int plantId)
        {
            var cartItem = await _repository.GetCartItemAsync(userId, plantId);
            if (cartItem == null)
                return new ResponseDTO<string> { StatusCode = 404, Message = "Item not in cart" };

            var stock = await _repository.GetPlantStockAsync(plantId);
            if (cartItem.Quantity >= stock)
                return new ResponseDTO<string> { StatusCode = 400, Message = "Cannot exceed stock" };

            await _repository.IncreaseQuantityAsync(userId, plantId);

            return new ResponseDTO<string> { StatusCode = 200, Message = "Quantity increased" };
        }

        public async Task<ResponseDTO<string>> DecreaseQuantityAsync(Guid userId, int plantId)
        {
            var cartItem = await _repository.GetCartItemAsync(userId, plantId);
            if (cartItem == null)
                return new ResponseDTO<string> { StatusCode = 404, Message = "Item not in cart" };

            if (cartItem.Quantity <= 1)
            {
                await _repository.RemoveCartItemAsync(userId, plantId);
                return new ResponseDTO<string> { StatusCode = 200, Message = "Item removed from cart" };
            }

            await _repository.DecreaseQuantityAsync(userId, plantId);
            return new ResponseDTO<string> { StatusCode = 200, Message = "Quantity decreased" };
        }

        public async Task<ResponseDTO<string>> IncreaseDurationAsync(Guid userId, int plantId)
        {
            var result = await _repository.IncreaseDurationAsync(userId, plantId);
            return result
                ? new ResponseDTO<string> { StatusCode = 200, Message = "Duration increased successfully" }
                : new ResponseDTO<string> { StatusCode = 404, Message = "Item not found in cart" };
        }

        public async Task<ResponseDTO<string>> DecreaseDurationAsync(Guid userId, int plantId)
        {
            var result = await _repository.DecreaseDurationAsync(userId, plantId);
            return result
                ? new ResponseDTO<string> { StatusCode = 200, Message = "Duration decreased successfully" }
                : new ResponseDTO<string> { StatusCode = 400, Message = "Minimum duration is 1 month" };
        }

        public async Task<ResponseDTO<CartResponseDTO>> GetCartItemsAsync(Guid userId)
        {
            var items = await _repository.GetCartItemsByUserAsync(userId);

            var totalCartPrice = items.Sum(x => x.TotalPrice);
            var totalItems = items.Count;

            var response = new CartResponseDTO
            {
                Items = items,
                CartSummary = new CartSummaryDTO
                {
                    TotalItems = totalItems,
                    TotalCartPrice = totalCartPrice
                }
            };

            return new ResponseDTO<CartResponseDTO>
            {
                StatusCode = 200,
                Message = "Cart fetched successfully",
                Data = response
            };
        }



    }
}
