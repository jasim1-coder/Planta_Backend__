﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces.Services
{
    public interface IRentalService
    {
        Task<ResponseDTO<PlaceRentalResponseDto>> PlaceRentalAsync(PlaceRentalRequestDto request);
    }
}
