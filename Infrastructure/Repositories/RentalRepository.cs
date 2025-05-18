using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Dapper;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly DapperContext _context;

        public RentalRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> VerifyCartTotal(Guid userId, decimal expectedTotal)
        {
            var sql = "SELECT SUM(total_price) FROM cart_items WHERE user_id = @UserId";
            using var connection = _context.CreateConnection();
            var total = await connection.ExecuteScalarAsync<decimal>(sql, new { UserId = userId });
            return total == expectedTotal;
        }

        public async Task<int> PlaceRentalAsync(PlaceRentalRequestDto request)
        {
            using var connection = _context.CreateConnection();
            using var transaction = connection.BeginTransaction();

            var rentalInsertSql = @"INSERT INTO rentals 
            (user_id, address_id, duration_in_months, total_price, rental_end_date)
            VALUES (@UserId, @AddressId, @Duration, @Total, DATE_ADD(NOW(), INTERVAL @Duration MONTH));
            SELECT LAST_INSERT_ID();";

            var rentalId = await connection.ExecuteScalarAsync<int>(rentalInsertSql, new
            {
                UserId = request.UserId,
                AddressId = request.AddressId,
                Duration = request.Items.Max(i => i.DurationMonths),
                Total = request.TotalPrice
            }, transaction);

            foreach (var item in request.Items)
            {
                var itemInsertSql = @"INSERT INTO rental_items 
                (rental_id, plant_id, seller_id, quantity, duration_months, unit_price, total_price, rental_end_date)
                VALUES (@RentalId, @PlantId, @SellerId, @Quantity, @Duration, @UnitPrice, @TotalPrice, DATE_ADD(NOW(), INTERVAL @Duration MONTH));";

                await connection.ExecuteAsync(itemInsertSql, new
                {
                    RentalId = rentalId,
                    PlantId = item.PlantId,
                    SellerId = item.SellerId,
                    Quantity = item.Quantity,
                    Duration = item.DurationMonths,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                }, transaction);

                var updateStockSql = "UPDATE plants SET quantity = quantity - @Qty WHERE id = @PlantId";
                await connection.ExecuteAsync(updateStockSql, new
                {
                    Qty = item.Quantity,
                    PlantId = item.PlantId
                }, transaction);
            }

            await transaction.CommitAsync();
            return rentalId;
        }
    }

}
