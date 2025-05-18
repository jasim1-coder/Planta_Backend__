using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IPlantRequestService
    {
        Task<ResponseDTO<string>> RequestPlantAsync(PlantRequestDTO dto);
        Task<ResponseDTO<List<PlantRequestViewDTO>>> GetAllPlantRequestsAsync();
        Task<ResponseDTO<PlantDTO>> AcceptPlantRequestAsync(int plantRequestId);
        Task<ResponseDTO<PlantDTO>> RejectPlantRequestAsync(int plantRequestId);
    }
}
