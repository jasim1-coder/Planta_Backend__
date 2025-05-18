using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DapperContext dapperContext;

        public AddressRepository(DapperContext _dapperContext)
        {
            dapperContext = _dapperContext;
        }

        public async Task<IEnumerable<Address>> GetAllAddressesByUserAsync(Guid userId)
        {
            const string sql = @"SELECT * FROM addresses WHERE user_id = @UserId";
            using var connection = dapperContext.CreateConnection();
            return await connection.QueryAsync<Address>(sql, new { UserId = userId });
        }

        public async Task AddAddressAsync(Guid userId, CreateAddressDTO dto)
        {
            const string sql = @"
                INSERT INTO addresses (user_id, name, phone_number, street, city, state, postal_code, country)
                VALUES (@UserId, @Name, @PhoneNumber, @Street, @City, @State, @PostalCode, @Country)";

            using var connection = dapperContext.CreateConnection();
            await ((DbConnection)connection).OpenAsync();
            using var tx = connection.BeginTransaction();

            await connection.ExecuteAsync(sql, new
            {
                UserId = userId,
                dto.Name,
                dto.PhoneNumber,
                dto.Street,
                dto.City,
                dto.State,
                dto.PostalCode,
                dto.Country
            }, tx);

            await ((DbTransaction)tx).CommitAsync();
        }

        public async Task DeleteAddressAsync(int addressId)
        {
            const string sql = @"DELETE FROM addresses WHERE id = @Id";
            using var connection = dapperContext.CreateConnection();
            await connection.ExecuteAsync(sql, new { Id = addressId });
        }
    }
}
