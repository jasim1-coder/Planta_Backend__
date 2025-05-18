using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{

        public interface IRoleRequestService
        {
            Task<ResponseDTO<object>> CreateSellerRoleRequest(CreateSellerRoleRequestDTO dto);
            Task<ResponseDTO<object>> CreateDeliveryPersonRoleRequest(CreateDeliveryPersonRoleRequestDTO dto);
            Task<ResponseDTO<object>> CreatePlantCaretakerRoleRequest(CreatePlantCaretakerRoleRequestDTO dto);
            Task<ResponseDTO<List<RoleRequestWithDetailsDTO>>> GetAllRoleRequestsWithMetadata();
            Task<ResponseDTO<object>> AcceptRoleRequestAsync(int roleRequestId);



    }



}
