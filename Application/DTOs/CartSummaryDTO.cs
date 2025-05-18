using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CartSummaryDTO
    {
        public int TotalItems { get; set; }
        public decimal TotalCartPrice { get; set; }
    }
}
