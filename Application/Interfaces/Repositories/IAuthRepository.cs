using System;
using System.Collections.Generic;
using System.Data;
using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<User> GetByUserNameAsync(string username);
        Task<User> GetByEmailAsync(string email);

        Task RegisterAsync(RegisterDTO regdata);


        Task<User> CheckRefreshToken(Guid userid);
        Task UpdateRefreshToken(Guid userid, string newrefreshtoken);
        //Task<User> GetByEmail(string email);

        //Task<Domain.Entities.User> GetUserByEmailAsync(string email, string passwordHash);
        Task<IList<string>> GetUserRolesAsync(Guid userId);
        Task AssignUserRoleAsync(Guid userId, string role);



    }
}
