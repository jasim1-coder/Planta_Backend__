using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
public interface IRoleRequestRepository
    {
        Task<int> CreateRoleRequestAsync(Guid userId, string role, int planId);
        Task InsertSellerDetailsAsync(int roleRequestId, CreateSellerRoleRequestDTO sellerDto);

        Task InsertDeliveryPersonDetailsAsync(int roleRequestId, CreateDeliveryPersonRoleRequestDTO dto);

        Task InsertPlantCaretakerDetailsAsync(int roleRequestId, CreatePlantCaretakerRoleRequestDTO dto);

        Task<List<RoleRequestWithDetailsDTO>> GetAllRoleRequestsWithDetailsAsync();

        Task<RoleRequest> GetByIdAsync(int id);
        Task UpdateAsync(RoleRequest request);

        Task UpdateUserRoleAsync(Guid userId, string role);
        Task AssignUserRoleAsync(Guid userId, string role);
        Task<int> CreateSubscriptionAsync(Subscription entity);
        Task<Plan?> GetByyIdAsync(int id);


    }

}
