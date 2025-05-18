using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<ResponseDTO<string>> AddToCartAsync(Guid userId, int plantId);
        Task<ResponseDTO<string>> RemoveFromCartAsync(Guid userId, int plantId);
        Task<ResponseDTO<string>> IncreaseQuantityAsync(Guid userId, int plantId);
        Task<ResponseDTO<string>> DecreaseQuantityAsync(Guid userId, int plantId);
        Task<ResponseDTO<string>> IncreaseDurationAsync(Guid userId, int plantId);
        Task<ResponseDTO<string>> DecreaseDurationAsync(Guid userId, int plantId);
        Task<ResponseDTO<CartResponseDTO>> GetCartItemsAsync(Guid userId);
    }
}
