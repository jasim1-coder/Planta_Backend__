
using Dapper;
using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Application.DTOs;
using Domain.Entities;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DapperContext dapperContext;
        public AuthRepository(DapperContext _dapperContext)
        {
            dapperContext = _dapperContext;
        }
        public async Task<User> GetByUserNameAsync(string username)
        {
            var sql = "Select * from Users where username=@UserName";
            using var connection = dapperContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserName = username });
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            const string sql = "SELECT * FROM Users WHERE email=@Email";
            using var conn = dapperContext.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task RegisterAsync(RegisterDTO regdata)
        {
            using var conn = dapperContext.CreateConnection();
            await ((DbConnection)conn).OpenAsync();
            using var tx = conn.BeginTransaction();

            const string insertUser = @"
        INSERT INTO users (UserID, UserName, Email, Name, Phone, PasswordHash, CreatedAt)
        VALUES (@UserId, @UserName, @Email, @Name, @Phone, @PasswordHash, NOW());";

            await conn.ExecuteAsync(insertUser, new
            {
                regdata.UserId,
                regdata.UserName,
                regdata.Email,
                regdata.Name,
                regdata.Phone,
                PasswordHash = regdata.Password // correctly mapped
            }, tx);

            const string insertRole = @"
        INSERT INTO user_roles (user_id, role_name)
        VALUES (@UserId, 'User');";
            await conn.ExecuteAsync(insertRole, new { regdata.UserId }, tx);

            await ((DbTransaction)tx).CommitAsync();
        }



        public async Task AssignUserRoleAsync(Guid userId, string role)
        {
            using var conn = dapperContext.CreateConnection();
            const string sql = @"
                INSERT INTO user_roles (user_id, role_name)
                SELECT @UserId,@Role
                WHERE NOT EXISTS(
                    SELECT 1 FROM user_roles WHERE user_id=@UserId AND role_name=@Role
                );";
            await conn.ExecuteAsync(sql, new { UserId = userId, Role = role });
        }


        public async Task<User> CheckRefreshToken(Guid userid)
        {
            var sql = "select * from Users where userid=@UserID";
            using var connection = dapperContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserID = userid });
        }

        public async Task UpdateRefreshToken(Guid userid, string newrefreshtoken)
        {
            var sql = "update Users set refreshtoken=@NewRefreshToken,refreshtokenexpiry=@RefreshExpiry where userid=@UserID";
            using var connection = dapperContext.CreateConnection();
            await connection.ExecuteAsync(sql, new { UserId = userid, NewRefreshToken = newrefreshtoken, RefreshExpiry = DateTime.Now.AddDays(7) });
        }

        public async Task<User> GetUserByEmailAsync(string email, string passwordHash)
        {
            const string sql = @"SELECT * FROM users WHERE email = @Email AND password_hash = @PasswordHash";
            using var conn = dapperContext.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(sql, new { Email = email, PasswordHash = passwordHash });
        }

        public async Task<IList<string>> GetUserRolesAsync(Guid userId)
        {
            const string sql = @"SELECT role_name FROM user_roles WHERE user_id = @UserId";
            using var conn = dapperContext.CreateConnection();
            var roles = await conn.QueryAsync<string>(sql, new { UserId = userId });
            return roles.AsList();
        }
    }
}
