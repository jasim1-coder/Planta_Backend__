using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PlaceRentalRequestDto
    {
        public Guid UserId { get; set; }
        public int AddressId { get; set; }
        public List<PlaceRentalItemDto> Items { get; set; }
        public decimal TotalPrice { get; set; } // From cart total
    }
}
