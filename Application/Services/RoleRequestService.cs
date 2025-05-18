using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RoleRequestService : IRoleRequestService
    {
        private readonly IRoleRequestRepository _repository;
        private readonly IMapper _mapper;
        public RoleRequestService(IRoleRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<object>> CreateSellerRoleRequest(CreateSellerRoleRequestDTO dto)
        {
            try
            {
                int roleRequestId = await _repository.CreateRoleRequestAsync(dto.UserId, "seller", dto.PlanId);
                await _repository.InsertSellerDetailsAsync(roleRequestId, dto);

                return new ResponseDTO<object>
                {
                    StatusCode = 201,
                    Data = new { RoleRequestId = roleRequestId },
                    Message = "Seller role request submitted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object>
                {
                    StatusCode = 500,
                    Message = "Error: " + ex.Message
                };
            }
        }


        public async Task<ResponseDTO<object>> CreateDeliveryPersonRoleRequest(CreateDeliveryPersonRoleRequestDTO dto)
        {
            try
            {
                int roleRequestId = await _repository.CreateRoleRequestAsync(dto.UserId, "delivery_person", dto.PlanId);
                await _repository.InsertDeliveryPersonDetailsAsync(roleRequestId, dto);

                return new ResponseDTO<object>
                {
                    StatusCode = 201,
                    Data = new { RoleRequestId = roleRequestId },
                    Message = "Delivery person role request submitted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object>
                {
                    StatusCode = 500,
                    Message = "Error: " + ex.Message
                };
            }
        }

        public async Task<ResponseDTO<object>> CreatePlantCaretakerRoleRequest(CreatePlantCaretakerRoleRequestDTO dto)
        {
            try
            {
                int roleRequestId = await _repository.CreateRoleRequestAsync(dto.UserId, "plant_caretaker", dto.PlanId);
                await _repository.InsertPlantCaretakerDetailsAsync(roleRequestId, dto);

                return new ResponseDTO<object>
                {
                    StatusCode = 201,
                    Data = new { RoleRequestId = roleRequestId },
                    Message = "Plant caretaker role request submitted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object>
                {
                    StatusCode = 500,
                    Message = "Error: " + ex.Message
                };
            }
        }

        // Application/Services/RoleRequestService.cs
        public async Task<ResponseDTO<List<RoleRequestWithDetailsDTO>>> GetAllRoleRequestsWithMetadata()
        {
            var result = await _repository.GetAllRoleRequestsWithDetailsAsync();
            return new ResponseDTO<List<RoleRequestWithDetailsDTO>>
            {
                StatusCode = 200,
                Data = result
            };
        }

        public async Task<ResponseDTO<object>> AcceptRoleRequestAsync(int roleRequestId)
        {
            try
            {
                // Fetch the role request by ID
                var request = await _repository.GetByIdAsync(roleRequestId);
                Console.WriteLine($"Role Request => Id: {request.Id}, UserId: {request.UserId}, RequestedRole: {request.RequestedRole}, Status: {request.Status}, ReviewedAt: {request.ReviewedAt}");

                // Check if the role request exists
                if (request == null)
                {
                    return new ResponseDTO<object> { Message = "Role request not found", StatusCode = 404 };
                }

                // Check if the requested role is missing
                if (string.IsNullOrEmpty(request.RequestedRole))
                {
                    return new ResponseDTO<object> { Message = "Requested role is missing", StatusCode = 400 };
                }

                // Check if the request is already approved
                if (request.Status == "approved")
                {
                    return new ResponseDTO<object> { Message = "Already approved", StatusCode = 400 };
                }

                // Update the status to "approved" and set the ReviewedAt timestamp
                request.Status = "approved";
                request.ReviewedAt = DateTime.UtcNow;

                // Update the role request in the repository
                await _repository.UpdateAsync(request);

                // Add the role to the user_roles table
                await _repository.AssignUserRoleAsync(request.UserId, request.RequestedRole);
                
                if(request.RequestedRole == "seller")
                {
                    var plan = await _repository.GetByyIdAsync(request.PlanId.Value)
    ?? throw new Exception("Plan not found.");

                    var now = DateTime.UtcNow;

                    // compute end date based on billing cycle
                    var endDate = plan.BillingCycle?.Trim().Equals("Monthly", StringComparison.OrdinalIgnoreCase) == true
                        ? now.AddMonths(1)
                        : now.AddYears(1);



                    var subscription = new Subscription
                    {
                        SellerId = request.UserId,
                        PlanId = (int)request.PlanId,
                        StartDate = DateTime.UtcNow,
                        EndDate = endDate, // assuming plan has DurationInDays
                        Status = "active",
                        NextBillingDate = endDate,
                        PaymentGateway = "razorpay", // or whatever applies
                        GatewaySubscriptionId = null,
                        CouponCode = null,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _repository.CreateSubscriptionAsync(subscription);
                }

                // Return a successful response
                return new ResponseDTO<object>
                {
                    Message = "Role request approved and role assigned to user",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // Log.Error(ex, "An error occurred while processing the role request.");

                return new ResponseDTO<object>
                {
                    Message = $"An error occurred: {ex.Message}",
                    StatusCode = 500
                };
            }
        }





    }

}

