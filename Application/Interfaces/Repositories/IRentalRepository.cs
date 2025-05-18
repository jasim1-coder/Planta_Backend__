using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces.Repositories
{
    public interface IRentalRepository
    {
        Task<int> PlaceRentalAsync(PlaceRentalRequestDto request);
        Task<bool> VerifyCartTotal(Guid userId, decimal expectedTotal);
    }
}
