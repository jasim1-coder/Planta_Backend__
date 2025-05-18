using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CartResponseDTO
    {
        public List<CartItemDTO> Items { get; set; } = new();
        public CartSummaryDTO CartSummary { get; set; }
    }
}
