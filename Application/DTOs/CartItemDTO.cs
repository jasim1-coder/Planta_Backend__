using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    //public class CartItemDTO
    //{
    //    public int Id { get; set; }
    //    public int PlantId { get; set; }
    //    public int Quantity { get; set; }
    //}
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public string PlantImage { get; set; } // Optional
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int DurationMonths { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity * DurationMonths;
    }


}
