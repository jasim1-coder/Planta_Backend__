using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class PlantRequestRepository : IPlantRequestRepository
    {
        private readonly DapperContext _context;
        public PlantRequestRepository(DapperContext context)
        {
            _context = context;
        }


        public async Task CreateAsync(PlantRequest request)
        {
            var sql = @"INSERT INTO plant_requests
                    (seller_id, category_id, name, price_per_month, description, care_instructions, stock, height_inches, pot_size_inches, benefits, requested_at, status,image_url)
                    VALUES
                    (@SellerId, @CategoryId, @Name, @PricePerMonth, @Description, @CareInstructions, @Stock, @HeightInches, @PotSizeInches, @Benefits, @RequestedAt, @Status,@ImageUrl);"
            ;
            using var connection = _context.CreateConnection();
              await connection.ExecuteAsync(sql, request);
        }

        public async Task<IEnumerable<PlantRequest>> GetAllRequestsAsync()
        {
            var sql = "SELECT * FROM plant_requests";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<PlantRequest>(sql);
        }

        public async Task<PlantRequest?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM plant_requests WHERE id = @id";
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<PlantRequest>(sql, new { id });
        }

        public async Task<int> CountApprovedPlantsBySellerAsync(Guid sellerId)
        {
            var sql = "SELECT COUNT(*) FROM plant_requests WHERE seller_id = @sellerId AND status = 'approved'";
            using var connection = _context.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(sql, new { sellerId });
        }

        public async Task<int> GetPlantLimitBySellerAsync(Guid sellerId)
        {
            var sql = @"SELECT p.no_of_plants 
                FROM subscriptions s
                JOIN plans p ON s.plan_id = p.id
                WHERE s.seller_id = @sellerId AND s.status = 'active'";

            using var connection = _context.CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { sellerId });

            int plantLimit = result ?? 0;

            Console.WriteLine($"[DEBUG] Seller ID: {sellerId} | Plant Limit Fetched: {plantLimit}");

            return plantLimit;
        }


        public async Task<bool> UpdateStatusAsync(int requestId, string status)
        {
            var sql = "UPDATE plant_requests SET status = @status, processed_at = NOW() WHERE id = @requestId";
            using var connection = _context.CreateConnection();

            var rows = await connection.ExecuteAsync(sql, new { status, requestId });
            return rows > 0;
        }

        public async Task<bool> InsertPlantAsync(Plant plant)
        {
            var sql = @"INSERT INTO plants (seller_id, category_id, name, price_per_month, description, care_instructions,
                    stock, height_inches, pot_size_inches, benefits, image_url, created_at)
                    VALUES (@SellerId, @CategoryId, @Name, @PricePerMonth, @Description, @CareInstructions,
                    @Stock, @HeightInches, @PotSizeInches, @Benefits, @ImageUrl, @CreatedAt)";
            using var connection = _context.CreateConnection();

            var result = await connection.ExecuteAsync(sql, plant);

            return result > 0;
        }

    }
}
