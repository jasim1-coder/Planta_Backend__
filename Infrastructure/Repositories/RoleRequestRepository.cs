using System.Data;
using Application.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Newtonsoft.Json;

namespace Infrastructure.Repositories
{
    public class RoleRequestRepository : IRoleRequestRepository
    {
        private readonly DapperContext _context;

        public RoleRequestRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateRoleRequestAsync(Guid userId, string role, int planId)
        {
            var query = @"
            INSERT INTO role_requests (user_id, requested_role, plan_id, status, requested_at)
            VALUES (@UserId, @RequestedRole, @PlanId, 'pending', NOW());
            SELECT LAST_INSERT_ID();";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, new
            {
                UserId = userId,
                RequestedRole = role,
                PlanId = planId
            });
        }

        public async Task InsertSellerDetailsAsync(int roleRequestId, CreateSellerRoleRequestDTO dto)
        {
            var query = @"
            INSERT INTO seller_details 
            (role_request_id, store_name, gst_number, business_email, phone_number, address)
            VALUES 
            (@RoleRequestId, @StoreName, @GstNumber, @BusinessEmail, @PhoneNumber, @Address);";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new
            {
                RoleRequestId = roleRequestId,
                dto.StoreName,
                dto.GstNumber,
                dto.BusinessEmail,
                dto.PhoneNumber,
                dto.Address
            });
        }

        public async Task InsertDeliveryPersonDetailsAsync(int roleRequestId, CreateDeliveryPersonRoleRequestDTO dto)
        {
            var query = @"
        INSERT INTO delivery_person_details 
        (role_request_id, vehicle_type, license_number, years_of_experience, phone_number)
        VALUES 
        (@RoleRequestId, @VehicleType, @LicenseNumber, @YearsOfExperience, @PhoneNumber);";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new
            {
                RoleRequestId = roleRequestId,
                dto.VehicleType,
                dto.LicenseNumber,
                dto.YearsOfExperience,
                dto.PhoneNumber
            });
        }


        public async Task InsertPlantCaretakerDetailsAsync(int roleRequestId, CreatePlantCaretakerRoleRequestDTO dto)
        {
            var query = @"
        INSERT INTO plant_caretaker_details 
        (role_request_id, specialization, certifications, years_of_experience)
        VALUES 
        (@RoleRequestId, @Specialization, @Certifications, @YearsOfExperience);";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new
            {
                RoleRequestId = roleRequestId,
                dto.Specialization,
                dto.Certifications,
                dto.YearsOfExperience
            });
        }

        // Infrastructure/Persistence/Repositories/RoleRequestRepository.cs
        public async Task<List<RoleRequestWithDetailsDTO>> GetAllRoleRequestsWithDetailsAsync()
        {
            var query = @"SELECT 
        rr.id AS Id,
        rr.user_id AS UserId,
        rr.requested_role AS RequestedRole,
        rr.status AS Status,
        rr.requested_at AS RequestedAt,
        rr.reviewed_at AS ReviewedAt,
        rr.plan_id AS PlanId,
        -- Seller details with unique aliases:
        sd.store_name AS SellerStoreName,
        sd.gst_number AS SellerGstNumber,
        sd.business_email AS SellerBusinessEmail,
        sd.phone_number AS SellerPhoneNumber,
        sd.address AS SellerAddress,
        -- Delivery Person details with unique aliases:
        dp.vehicle_type AS DeliveryVehicleType,
        dp.license_number AS DeliveryLicenseNumber,
        dp.years_of_experience AS DeliveryYearsOfExperience,
        dp.phone_number AS DeliveryPhoneNumber,
        -- Plant Caretaker details with unique aliases:
        pc.specialization AS CaretakerSpecialization,
        pc.certifications AS CaretakerCertifications,
        pc.years_of_experience AS CaretakerYearsOfExperience
    FROM role_requests rr
    LEFT JOIN seller_details sd ON rr.id = sd.role_request_id
    LEFT JOIN delivery_person_details dp ON rr.id = dp.role_request_id
    LEFT JOIN plant_caretaker_details pc ON rr.id = pc.role_request_id";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<RoleRequestWithDetailsDTO, SellerDetailDTO, DeliveryPersonDetailDTO, PlantCaretakerDetailDTO, RoleRequestWithDetailsDTO>(
                query,
                (role, seller, delivery, caretaker) =>
                {
                    role.SellerDetails = (seller != null && !string.IsNullOrEmpty(seller.SellerStoreName)) ? seller : null;
                    role.DeliveryPersonDetails = (delivery != null && !string.IsNullOrEmpty(delivery.DeliveryVehicleType)) ? delivery : null;
                    role.PlantCaretakerDetails = (caretaker != null && !string.IsNullOrEmpty(caretaker.CaretakerSpecialization)) ? caretaker : null;
                    return role;
                },
                splitOn: "SellerStoreName,DeliveryVehicleType,CaretakerSpecialization"
            );
            return result.AsList();
        }

        public async Task<RoleRequest> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM role_requests WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<RoleRequest>(sql, new { Id = id });
        }

        public async Task UpdateAsync(RoleRequest request)
        {
            var sql = @"UPDATE role_requests 
                    SET status = @Status, reviewed_at = @ReviewedAt 
                    WHERE id = @Id";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, request);
        }
        public async Task AssignUserRoleAsync(Guid userId, string role)
        {
                var sql = @"INSERT INTO user_roles (user_id, role_name)
                    VALUES (@UserId, @RoleName);";

                using var connection = _context.CreateConnection();
                await connection.ExecuteAsync(sql, new { UserId = userId, RoleName = role });
            
           
        }


        public async Task UpdateUserRoleAsync(Guid userId, string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentException("Role cannot be null or empty.", nameof(role));
            }

            var query = @"INSERT INTO user_roles (user_id, role_name)
                  SELECT @UserId, @Role
                  WHERE NOT EXISTS (
                      SELECT 1 FROM user_roles 
                      WHERE user_id = @UserId AND role_name = @Role
                  );";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { UserId = userId, Role = role });
        }

        public async Task<Plan?> GetByyIdAsync(int id)
        {
            var sql = @"
              SELECT id, name, billing_cycle AS BillingCycle,
                     price
                FROM plans
               WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Plan>(sql, new { Id = id });

        }

        public async Task<int> CreateSubscriptionAsync(Subscription entity)
        {
            var sql = @"
            INSERT INTO subscriptions
              (seller_id, plan_id, start_date, end_date, status, trial_end_date,
               next_billing_date, payment_gateway, gateway_subscription_id,
               coupon_code, created_at, updated_at)
            VALUES
              (@SellerId, @PlanId, @StartDate, @EndDate, @Status, @TrialEndDate,
               @NextBillingDate, @PaymentGateway, @GatewaySubscriptionId,
               @CouponCode, @CreatedAt, @UpdatedAt);
            SELECT LAST_INSERT_ID();";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, entity);
        }


    }

}
