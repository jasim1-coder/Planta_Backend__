using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Services;
using AutoMapper;

namespace Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public RentalService(IRentalRepository rentalRepository, IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<PlaceRentalResponseDto>> PlaceRentalAsync(PlaceRentalRequestDto request)
        {
            var isValid = await _rentalRepository.VerifyCartTotal(request.UserId, request.TotalPrice);
            if (!isValid)
            {
                return new ResponseDTO<PlaceRentalResponseDto>
                {
                    StatusCode = 400,
                    Message = "Cart total mismatch",
                    Error = "Cart total does not match the expected value",
                    Data = null
                };
            }

            var rentalId = await _rentalRepository.PlaceRentalAsync(request);
            var responseData = new PlaceRentalResponseDto
            {
                RentalId = rentalId,
                Message = "Rental placed successfully"
            };

            return new ResponseDTO<PlaceRentalResponseDto>
            {
                StatusCode = 200,
                Message = "Success",
                Data = responseData,
                Error = null
            };
        }
    }

}
