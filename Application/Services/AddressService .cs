using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;

namespace Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repo;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<IEnumerable<AddressDTO>>> GetAddressesAsync(Guid userId)
        {
            var addresses = await _repo.GetAllAddressesByUserAsync(userId);
            var result = _mapper.Map<IEnumerable<AddressDTO>>(addresses);

            return new ResponseDTO<IEnumerable<AddressDTO>>
            {
                StatusCode = 200,
                Message = "Addresses retrieved successfully",
                Data = result
            };
        }

        public async Task<ResponseDTO<string>> AddAddressAsync(Guid userId, CreateAddressDTO dto)
        {
            await _repo.AddAddressAsync(userId, dto);

            return new ResponseDTO<string>
            {
                StatusCode = 200,
                Message = "Address added successfully"
            };
        }

        public async Task<ResponseDTO<string>> DeleteAddressAsync(int addressId)
        {
            await _repo.DeleteAddressAsync(addressId);

            return new ResponseDTO<string>
            {
                StatusCode = 200,
                Message = "Address deleted successfully"
            };
        }
    }

}
