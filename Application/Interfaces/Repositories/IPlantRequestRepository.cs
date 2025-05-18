using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPlantRequestRepository
    {
        Task CreateAsync(PlantRequest request);
        Task<IEnumerable<PlantRequest>> GetAllRequestsAsync();

        Task<PlantRequest?> GetByIdAsync(int id);
        Task<int> CountApprovedPlantsBySellerAsync(Guid sellerId);
        Task<int> GetPlantLimitBySellerAsync(Guid sellerId);
        Task<bool> UpdateStatusAsync(int requestId, string status);
        Task<bool> InsertPlantAsync(Plant plant);
    }
}
