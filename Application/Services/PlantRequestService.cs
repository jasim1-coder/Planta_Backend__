using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class PlantRequestService : IPlantRequestService
    {
        private readonly IPlantRequestRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinary;


        public PlantRequestService(IPlantRequestRepository repository, IMapper mapper, ICloudinaryService cloudinary)
        {
            _repository = repository;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        public async Task<ResponseDTO<string>> RequestPlantAsync(PlantRequestDTO dto)
        {
            try
            {
                var entity = _mapper.Map<PlantRequest>(dto);
                if (dto.Image != null)
                {
                    var imageUrl = await _cloudinary.UploadImageAsync(dto.Image);
                    entity.ImageUrl = imageUrl;
                }
                await _repository.CreateAsync(entity);

                return new ResponseDTO<string>
                {
                    StatusCode = 201,
                    Message = "Plant request submitted successfully.",
                    Data = "Created",
                    Error = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>
                {
                    StatusCode = 500,
                    Message = "Failed to request plant.",
                    Data = null,
                    Error = ex.Message
                };
            }
        }

        public async Task<ResponseDTO<List<PlantRequestViewDTO>>> GetAllPlantRequestsAsync()
        {
            try
            {
                var entities = await _repository.GetAllRequestsAsync();
                var result = _mapper.Map<List<PlantRequestViewDTO>>(entities);

                return new ResponseDTO<List<PlantRequestViewDTO>>
                {
                    StatusCode = 200,
                    Message = "Plant requests retrieved.",
                    Data = result,
                    Error = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<PlantRequestViewDTO>>
                {
                    StatusCode = 500,
                    Message = "Error retrieving plant requests.",
                    Data = null,
                    Error = ex.Message
                };
            }
        }

        public async Task<ResponseDTO<PlantDTO>> AcceptPlantRequestAsync(int requestId)
        {
            var plantRequest = await _repository.GetByIdAsync(requestId);
            if (plantRequest == null)
            {
                return new ResponseDTO<PlantDTO>
                {
                    StatusCode = 404,
                    Message = "Plant request not found",
                    Data = null
                };
            }

            // Check the seller's current number of approved plants and their plan limit
            var sellerId = plantRequest.SellerId;
            var approvedPlantsCount = await _repository.CountApprovedPlantsBySellerAsync(sellerId);
            var plantLimit = await _repository.GetPlantLimitBySellerAsync(sellerId);

            if (approvedPlantsCount >= plantLimit)
            {
                Console.WriteLine($"Approved count: {approvedPlantsCount}, Limit: {plantLimit}");
                Console.WriteLine($"Approved count: {approvedPlantsCount}, Limit: {plantLimit}");
                Console.WriteLine($"Approved count: {approvedPlantsCount}, Limit: {plantLimit}");
                Console.WriteLine($"Approved count: {approvedPlantsCount}, Limit: {plantLimit}");
                Console.WriteLine($"Approved count: {approvedPlantsCount}, Limit: {plantLimit}");
                Console.WriteLine($"Approved count: {approvedPlantsCount}, Limit: {plantLimit}");
                return new ResponseDTO<PlantDTO>
                {
                    StatusCode = 400,
                    Message = "Plant request exceeds the plant limit for this seller.",
                    Data = null
                };
            }

            // Map the plant request to the Plant entity
            var plant = _mapper.Map<Plant>(plantRequest);

            // Insert the plant into the plant table
            var isInserted = await _repository.InsertPlantAsync(plant);
            if (!isInserted)
            {
                return new ResponseDTO<PlantDTO>
                {
                    StatusCode = 500,
                    Message = "Failed to accept plant request",
                    Data = null
                };
            }

            // Update the plant request status to 'accepted'
            var isStatusUpdated = await _repository.UpdateStatusAsync(requestId, "accepted");
            if (!isStatusUpdated)
            {



                return new ResponseDTO<PlantDTO>
                {
                    StatusCode = 500,
                    Message = "Failed to update the status of the plant request",
                    Data = null
                };
            }

            // Map the inserted plant to a PlantDTO and return
            var plantDTO = _mapper.Map<PlantDTO>(plant);
            return new ResponseDTO<PlantDTO>
            {
                StatusCode = 200,
                Message = "Plant request accepted successfully",
                Data = plantDTO
            };
        }

        public async Task<ResponseDTO<PlantDTO>> RejectPlantRequestAsync(int requestId)
        {
            var plantRequest = await _repository.GetByIdAsync(requestId);
            if (plantRequest == null)
            {
                return new ResponseDTO<PlantDTO>
                {
                    StatusCode = 404,
                    Message = "Plant request not found",
                    Data = null
                };
            }

            // Update the plant request status to 'rejected'
            var isStatusUpdated = await _repository.UpdateStatusAsync(requestId, "rejected");
            if (!isStatusUpdated)
            {
                return new ResponseDTO<PlantDTO>
                {
                    StatusCode = 500,
                    Message = "Failed to update the status of the plant request",
                    Data = null
                };
            }

            return new ResponseDTO<PlantDTO>
            {
                StatusCode = 200,
                Message = "Plant request rejected successfully",
                Data = null
            };
        }
    }
}