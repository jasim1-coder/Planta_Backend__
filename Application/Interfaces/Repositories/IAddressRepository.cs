using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAllAddressesByUserAsync(Guid userId);
        Task AddAddressAsync(Guid userId, CreateAddressDTO dto);
        Task DeleteAddressAsync(int addressId);
    }

}
