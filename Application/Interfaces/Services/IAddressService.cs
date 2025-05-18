using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IAddressService
    {
        Task<ResponseDTO<IEnumerable<AddressDTO>>> GetAddressesAsync(Guid userId);
        Task<ResponseDTO<string>> AddAddressAsync(Guid userId, CreateAddressDTO dto);
        Task<ResponseDTO<string>> DeleteAddressAsync(int addressId);
    }

}
